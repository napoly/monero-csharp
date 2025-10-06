using Monero.Common;
using Monero.Daemon.Common;
using Monero.Wallet.Common;
using Monero.Wallet.Rpc;

using MoneroJsonRpcParams = System.Collections.Generic.Dictionary<string, object?>;

namespace Monero.Wallet;

public class MoneroWalletRpc : IMoneroWallet
{
    // class variables
    private const int ErrorCodeInvalidPaymentId = -5; // invalid payment id error code

    private const ulong
        DefaultSyncPeriodInMs =
            20000; // default period between syncs in ms (defined by DEFAULT_AUTO_REFRESH_PERIOD in wallet_rpc_server.cpp)

    private readonly Dictionary<uint, Dictionary<uint, string?>?>
        _addressCache = []; // cache static addresses to reduce requests

    private readonly List<MoneroWalletListener> _listeners = [];

    private readonly MoneroRpcConnection _rpc; // rpc connection to monero-wallet-rpc

    // instance variables
    private readonly SemaphoreSlim _syncSem = new(1, 1); // lock for synchronizing sync requests
    private MoneroRpcConnection? _daemonConnection; // current daemon connection (unknown/null until explicitly set)
    private string? _path; // wallet's path identifier
    private ulong _syncPeriodInMs = DefaultSyncPeriodInMs; // period between syncs in ms (default 20000)

    private MoneroWalletPoller? _walletPoller; // listener which polls monero-wallet-rpc

    public MoneroWalletRpc(MoneroRpcConnection connection)
    {
        _rpc = connection;
    }

    public List<MoneroWalletListener> GetListeners()
    {
        return [.. _listeners];
    }

    #region RPC Wallet Methods

    public async Task OpenWallet(MoneroWalletConfig config)
    {
        // validate config
        if (config == null)
        {
            throw new MoneroError("Must provide configuration of wallet to open");
        }

        if (string.IsNullOrEmpty(config.GetPath()))
        {
            throw new MoneroError("Filename is not initialized");
        }
        // TODO: ensure other fields are uninitialized?

        // open wallet on rpc server
        MoneroJsonRpcParams parameters = [];
        parameters.Add("filename", config.GetPath());
        parameters.Add("password", config.GetPassword() ?? "");
        await _rpc.SendJsonRequest<MoneroRpcResponse>("open_wallet", parameters);
        Clear();
        _path = config.GetPath();

        if (config.GetServer() != null)
        {
            await SetDaemonConnection(config.GetServer(), true, null);
        }
    }

    public async Task OpenWallet(string path, string? password = null)
    {
        await OpenWallet(new MoneroWalletConfig().SetPath(path).SetPassword(password));
    }

    public async Task CreateWallet(MoneroWalletConfig config)
    {
        // validate config
        if (config == null)
        {
            throw new MoneroError("Must specify config to create wallet");
        }

        if (config.GetNetworkType() != null)
        {
            throw new MoneroError("Cannot specify network type when creating RPC wallet");
        }

        if (config.GetSeed() != null && (config.GetPrimaryAddress() != null || config.GetPrivateViewKey() != null ||
                                         config.GetPrivateSpendKey() != null))
        {
            throw new MoneroError("Wallet can be initialized with a seed or keys but not both");
        }

        if (config.GetAccountLookahead() != null || config.GetSubaddressLookahead() != null)
        {
            throw new MoneroError(
                "monero-wallet-rpc does not support creating wallets with subaddress lookahead over rpc");
        }

        // create wallet
        if (config.GetSeed() != null)
        {
            await CreateWalletFromSeed(config);
        }
        else if (config.GetPrivateSpendKey() != null || config.GetPrimaryAddress() != null)
        {
            await CreateWalletFromKeys(config);
        }
        else
        {
            await CreateWalletRandom(config);
        }

        if (config.GetServer() != null)
        {
            await SetDaemonConnection(config.GetServer(), true, null);
        }
    }

    private async Task CreateWalletRandom(MoneroWalletConfig? config)
    {
        if (config == null)
        {
            throw new MoneroError("Config is null");
        }

        // validate and normalize config
        config = config.Clone();
        if (config.GetSeedOffset() != null)
        {
            throw new MoneroError("Cannot specify seed offset when creating random wallet");
        }

        if (config.GetRestoreHeight() != null)
        {
            throw new MoneroError("Cannot specify restore height when creating random wallet");
        }

        if (config.GetSaveCurrent() == false)
        {
            throw new MoneroError("Current wallet is saved automatically when creating random wallet");
        }

        if (string.IsNullOrEmpty(config.GetPath()))
        {
            throw new MoneroError("Wallet name is not initialized");
        }

        MoneroJsonRpcParams parameters = PrepareWalletCreationParameters(config);
        try { await _rpc.SendJsonRequest<MoneroRpcResponse>("create_wallet", parameters); }
        catch (MoneroRpcError e) { HandleCreateWalletError(config.GetPath(), e); }

        Clear();
        _path = config.GetPath();
    }

    private static MoneroJsonRpcParams PrepareWalletCreationParameters(MoneroWalletConfig config)
    {
        if (string.IsNullOrEmpty(config.GetLanguage()))
        {
            config.SetLanguage(IMoneroWallet.DefaultLanguage);
        }

        // send request
        MoneroJsonRpcParams parameters = [];
        parameters.Add("filename", config.GetPath());
        parameters.Add("password", config.GetPassword());
        parameters.Add("language", config.GetLanguage());
        return parameters;
    }

    private async Task CreateWalletFromSeed(MoneroWalletConfig config)
    {
        config = config.Clone();
        MoneroJsonRpcParams parameters = PrepareWalletCreationParameters(config);
        parameters.Add("seed", config.GetSeed());
        parameters.Add("seed_offset", config.GetSeedOffset());
        parameters.Add("restore_height", config.GetRestoreHeight());
        parameters.Add("autosave_current", config.GetSaveCurrent());
        parameters.Add("enable_multisig_experimental", config.IsMultisig());

        try { await _rpc.SendJsonRequest<MoneroRpcResponse>("restore_deterministic_wallet", parameters); }
        catch (MoneroRpcError e) { HandleCreateWalletError(config.GetPath(), e); }

        Clear();
        _path = config.GetPath();
    }

    private async Task CreateWalletFromKeys(MoneroWalletConfig config)
    {
        config = config.Clone();
        if (config.GetSeedOffset() != null)
        {
            throw new MoneroError("Cannot specify seed offset when creating wallet from keys");
        }

        if (config.GetRestoreHeight() == null)
        {
            config.SetRestoreHeight(0);
        }

        MoneroJsonRpcParams parameters = [];

        parameters.Add("filename", config.GetPath());
        parameters.Add("password", config.GetPassword());
        parameters.Add("address", config.GetPrimaryAddress());
        parameters.Add("viewkey", config.GetPrivateViewKey());
        parameters.Add("spendkey", config.GetPrivateSpendKey());
        parameters.Add("restore_height", config.GetRestoreHeight());
        parameters.Add("autosave_current", config.GetSaveCurrent());

        try { await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("generate_from_keys", parameters); }
        catch (MoneroRpcError e) { HandleCreateWalletError(config.GetPath(), e); }

        Clear();
        _path = config.GetPath();
    }

    private static void HandleCreateWalletError(string? name, MoneroRpcError? e)
    {
        if (e == null)
        {
            throw new MoneroRpcError("Cannot create wallet due an unknown error");
        }

        if (e.Message.Equals("Cannot create wallet. Already exists."))
        {
            throw new MoneroRpcError("Wallet already exists: " + (name ?? "unkown"), e.GetCode(), e.GetRpcMethod(),
                e.GetRpcParams());
        }

        if (e.Message.Equals("Electrum-style word list failed verification"))
        {
            throw new MoneroRpcError("Invalid mnemonic", e.GetCode(), e.GetRpcMethod(), e.GetRpcParams());
        }

        throw e;
    }

    private static void HandleInvalidTxIdError(MoneroRpcError? e)
    {
        if (e == null)
        {
            throw new MoneroError("Cannot parse invalid tx id error");
        }

        if (-8 == e.GetCode() && e.Message.Contains("TX ID has invalid format"))
        {
            e = new MoneroRpcError("TX hash has invalid format", e.GetCode(), e.GetRpcMethod(),
                e.GetRpcParams()); // normalize error message
        }

        throw new MoneroRpcError(e);
    }

    #endregion

    #region Common Wallet Methods

    public void AddListener(MoneroWalletListener listener)
    {
        lock (_listeners)
        {
            if (listener == null)
            {
                throw new MoneroError("Cannot add null listener");
            }

            _listeners.Add(listener);
            RefreshListening();
        }
    }

    public void RemoveListener(MoneroWalletListener listener)
    {
        lock (_listeners)
        {
            _listeners.Remove(listener);
            RefreshListening();
        }
    }

    public async Task<bool> IsViewOnly()
    {
        try
        {
            MoneroJsonRpcParams parameters = [];
            parameters.Add("key_type", "mnemonic");
            await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("query_key", parameters);
            return false; // key retrieval succeeds if not view-only
        }
        catch (MoneroError e)
        {
            if (-29 == e.GetCode())
            {
                return true; // wallet is view-only
            }

            if (-1 == e.GetCode())
            {
                return false; // wallet is offline but not view-only
            }

            throw;
        }
    }

    public async Task SetDaemonConnection(MoneroRpcConnection? connection, bool? isTrusted, SslOptions? sslOptions)
    {
        if (sslOptions == null)
        {
            sslOptions = new SslOptions();
        }

        MoneroJsonRpcParams parameters = [];
        parameters.Add("address", connection == null ? "placeholder" : connection.GetUri());
        parameters.Add("username", connection == null ? "" : connection.GetUsername());
        parameters.Add("password", connection == null ? "" : connection.GetPassword());
        parameters.Add("trusted", isTrusted);
        parameters.Add("ssl_support", "autodetect");
        parameters.Add("ssl_private_key_path", sslOptions.GetPrivateKeyPath());
        parameters.Add("ssl_certificate_path", sslOptions.GetCertificatePath());
        parameters.Add("ssl_ca_file", sslOptions.GetCertificateAuthorityFile());
        parameters.Add("ssl_allowed_fingerprints", sslOptions.GetAllowedFingerprints());
        parameters.Add("ssl_allow_any_cert", sslOptions.GetAllowAnyCert());
        await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("set_daemon", parameters);
        if (connection != null && !string.IsNullOrEmpty(connection.GetUri()))
        {
            _daemonConnection = new MoneroRpcConnection(
                new Uri(connection.GetUri()),
                connection.GetUsername(),
                connection.GetPassword()
            );
        }
        else
        {
            _daemonConnection = null;
        }
    }

    public Task SetProxyUri(string? uri)
    {
        throw new MoneroError(
            "MoneroWalletRpc.SetProxyUri() not supported. Start monero-wallet-rpc with --proxy instead.");
    }

    public Task<MoneroRpcConnection?> GetDaemonConnection()
    {
        return Task.FromResult(_daemonConnection);
    }

    public async Task<bool> IsConnectedToDaemon()
    {
        try
        {
            await CheckReserveProof(await GetPrimaryAddress(), "",
                ""); // TODO (monero-project): provide better way to know if wallet rpc is connected to daemon
            throw new Exception("check reserve expected to fail");
        }
        catch (MoneroError e)
        {
            return !e.Message.Contains("Failed to connect to daemon");
        }
    }

    public async Task<MoneroVersion> GetVersion()
    {
        return await _rpc.SendCommandAsync<NoRequestModel, MoneroVersion>("get_version", NoRequestModel.Instance);
    }

    public Task<string> GetPath()
    {
        return Task.FromResult(_path ?? "");
    }

    public async Task<string> GetSeed()
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("key_type", "mnemonic");
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, QueryKeyResult>("query_key", parameters);
        return result.Key;
    }

    public Task<string> GetSeedLanguage()
    {
        throw new MoneroError("MoneroWalletRpc.GetSeedLanguage() not supported");
    }

    public async Task<string> GetPrivateViewKey()
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("key_type", "view_key");
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, QueryKeyResult>("query_key", parameters);
        return result.Key;
    }

    public async Task<string> GetPublicViewKey()
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("key_type", "public_view_key");
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, QueryKeyResult>("query_key", parameters);
        return result.Key;
    }

    public async Task<string> GetPublicSpendKey()
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("key_type", "public_spend_key");
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, QueryKeyResult>("query_key", parameters);
        return result.Key;
    }

    public async Task<string> GetPrivateSpendKey()
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("key_type", "spend_key");
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, QueryKeyResult>("query_key", parameters);
        return result.Key;
    }

    public async Task<string?> GetAddress(uint accountIdx, uint subaddressIdx)
    {
        Dictionary<uint, string?>? subaddressMap = _addressCache.GetValueOrDefault(accountIdx);
        if (subaddressMap == null)
        {
            await GetSubaddresses(accountIdx, true, null); // cache all addresses at this account
            return await GetAddress(accountIdx, subaddressIdx); // uses cache
        }

        string? address = subaddressMap.GetValueOrDefault(subaddressIdx);
        if (address == null)
        {
            await GetSubaddresses(accountIdx, true, null); // cache all addresses at this account
            Dictionary<uint, string?>? map = _addressCache.GetValueOrDefault(accountIdx);
            return map?.GetValueOrDefault(subaddressIdx);
        }

        return address;
    }

    public async Task<MoneroSubaddress> GetAddressIndex(string address)
    {
        // fetch result and normalize error if address does not belong to the wallet
        GetAddressIndexResult result;
        try
        {
            MoneroJsonRpcParams parameters = [];
            parameters.Add("address", address);
            result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, GetAddressIndexResult>("get_address_index", parameters);
        }
        catch (MoneroRpcError e)
        {
            MoneroUtils.Log(0, e.Message);
            if (-2 == e.GetCode())
            {
                throw new MoneroError(e.Message, e.GetCode());
            }

            throw;
        }

        return result.Index;
    }

    public async Task<MoneroIntegratedAddress> GetIntegratedAddress(string? standardAddress, string? paymentId)
    {
        try
        {
            MoneroJsonRpcParams parameters = [];
            parameters.Add("standard_address", standardAddress);
            parameters.Add("payment_id", paymentId);
            var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, MakeIntegratedAddressResult>("make_integrated_address", parameters);
            return await DecodeIntegratedAddress(result.IntegratedAddress);
        }
        catch (MoneroRpcError e)
        {
            if (e.Message.Contains("Invalid payment ID"))
            {
                throw new MoneroError("Invalid payment ID: " + paymentId, ErrorCodeInvalidPaymentId);
            }

            throw;
        }
    }

    public async Task<MoneroIntegratedAddress> DecodeIntegratedAddress(string integratedAddress)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("integrated_address", integratedAddress);
        var result =
            await _rpc.SendCommandAsync<MoneroJsonRpcParams, SplitIntegratedAddressResult>("split_integrated_address", parameters);
        return new MoneroIntegratedAddress(result.Address, result.PaymentId,
            integratedAddress);
    }

    public async Task<ulong> GetHeight()
    {
        var response = await _rpc.SendCommandAsync<NoRequestModel, GetHeightResult>("get_height", NoRequestModel.Instance);
        return response.Height;
    }

    public Task<ulong> GetDaemonHeight()
    {
        throw new MoneroError("monero-wallet-rpc does not support getting the chain height");
    }

    public Task<ulong> GetHeightByDate(int year, int month, int day)
    {
        throw new MoneroError("monero-wallet-rpc does not support getting a height by date");
    }

    public async Task<MoneroSyncResult> Sync(ulong? startHeight, MoneroWalletListener? listener)
    {
        if (listener != null)
        {
            throw new MoneroError("Monero Wallet RPC does not support reporting sync progress");
        }

        MoneroJsonRpcParams parameters = [];
        parameters.Add("start_height", startHeight);
        // TODO (monero-project): monero-wallet-rpc hangs at 100% cpu utilization if refresh called concurrently
        await _syncSem.WaitAsync();

        try
        {
            var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroSyncResult>("refresh", parameters);
            await Poll();
            _syncSem.Release();
            return result;
        }
        catch (MoneroError err)
        {
            _syncSem.Release();

            if (err.Message.Equals("no connection to daemon"))
            {
                throw new MoneroError("Wallet is not connected to daemon");
            }

            throw;
        }
    }

    public async Task StartSyncing(ulong? syncPeriodInMs)
    {
        // convert ms to seconds for rpc parameter
        ulong syncPeriodInSeconds = (syncPeriodInMs ?? DefaultSyncPeriodInMs) / 1000;

        // send rpc request
        MoneroJsonRpcParams parameters = [];
        parameters.Add("enable", true);
        parameters.Add("period", syncPeriodInSeconds);
        await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("auto_refresh", parameters);

        // update sync period for poller
        _syncPeriodInMs = syncPeriodInSeconds * 1000;
        if (_walletPoller != null)
        {
            _walletPoller.SetPeriodInMs(_syncPeriodInMs);
        }

        // poll if listening
        await Poll();
    }

    public async Task StopSyncing()
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("enable", false);
        await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("auto_refresh", parameters);
    }

    public async Task ScanTxs(List<string> txHashes)
    {
        if (txHashes == null || txHashes.Count == 0)
        {
            throw new MoneroError("No tx hashes given to scan");
        }

        MoneroJsonRpcParams parameters = [];
        parameters.Add("txids", txHashes);
        await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("scan_tx", parameters);
        await Poll(); // notify of changes
    }

    public async Task RescanSpent()
    {
        await _rpc.SendCommandAsync<NoRequestModel, MoneroRpcResponse>("rescan_spent", NoRequestModel.Instance);
    }

    public async Task RescanBlockchain()
    {
        await _rpc.SendCommandAsync<NoRequestModel, MoneroRpcResponse>("rescan_blockchain", NoRequestModel.Instance);
    }

    public async Task<ulong> GetBalance(uint? accountIdx, uint? subaddressIdx)
    {
        return (await GetBalances(accountIdx, subaddressIdx))[0];
    }

    public async Task<ulong> GetUnlockedBalance(uint? accountIdx, uint? subaddressIdx)
    {
        return (await GetBalances(accountIdx, subaddressIdx))[1];
    }

    private static void InitSubaddress(MoneroSubaddress? tgtSubaddress, MoneroSubaddress? subaddress)
    {
        if (tgtSubaddress == null || subaddress == null)
        {
            throw new MoneroError("Cannot initialize null subaddress");
        }

        if (subaddress.GetBalance() != null)
        {
            tgtSubaddress.SetBalance(subaddress.GetBalance());
        }

        if (subaddress.GetUnlockedBalance() != null)
        {
            tgtSubaddress.SetUnlockedBalance(subaddress.GetUnlockedBalance());
        }

        if (subaddress.GetNumUnspentOutputs() != null)
        {
            tgtSubaddress.SetNumUnspentOutputs(subaddress.GetNumUnspentOutputs());
        }

        if (subaddress.GetNumBlocksToUnlock() != null)
        {
            tgtSubaddress.SetNumBlocksToUnlock(subaddress.GetNumBlocksToUnlock());
        }
    }

    public async Task<List<MoneroAccount>> GetAccounts(bool includeSubaddresses, bool skipBalances, string? tag)
    {
        // fetch accounts from rpc
        MoneroJsonRpcParams parameters = [];
        parameters.Add("tag", tag);
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, GetAccountsResult>("get_accounts", parameters);

        // build account objects and fetch subaddresses per account using get_address
        // TODO monero-wallet-rpc: get_address should support all_accounts so not called once per account
        List<MoneroAccount> accounts = result.Accounts;

        if (includeSubaddresses)
        {
            foreach (MoneroAccount account in accounts)
            {
                account.Subaddresses = await GetSubaddresses((uint)account.Index!, true, null);
            }
        }

        if (includeSubaddresses && !skipBalances)
        {
            // these fields are not initialized if the subaddress is unused and therefore not returned from `get_balance`
            foreach (MoneroAccount account in accounts)
            {
                foreach (MoneroSubaddress subaddress in account.Subaddresses!)
                {
                    subaddress.SetBalance(0);
                    subaddress.SetUnlockedBalance(0);
                    subaddress.SetNumUnspentOutputs(0);
                    subaddress.SetNumBlocksToUnlock(0);
                }
            }

            // fetch and merge info from get_balance
            parameters.Clear();
            parameters.Add("all_accounts", true);
            var getBalanceResult = await _rpc.SendCommandAsync<MoneroJsonRpcParams, GetBalanceResult>("get_balance", parameters);

            foreach (RpcBalanceInfo rpcSubaddress in getBalanceResult.PerSubaddress)
            {
                MoneroSubaddress subaddress = ConvertRpcSubaddress(rpcSubaddress);

                // merge info
                MoneroAccount account = accounts[(int)subaddress.AccountIndex!];
                if (account.Index != subaddress.AccountIndex)
                {
                    throw new MoneroError("RPC accounts are out of order"); // would need to switch lookup to loop
                }

                MoneroSubaddress tgtSubaddress = account.Subaddresses![(int)subaddress.Index!];
                if (tgtSubaddress.Index != subaddress.Index)
                {
                    throw new MoneroError("RPC subaddresses are out of order");
                }

                InitSubaddress(tgtSubaddress, subaddress);
            }
        }

        // return accounts
        return accounts;
    }

    public async Task<MoneroAccount> CreateAccount(string? label)
    {
        label = string.IsNullOrEmpty(label) ? null : label;
        MoneroJsonRpcParams parameters = [];
        parameters.Add("label", label);
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, AccountInfo>("create_account", parameters);
        return result == null
            ? throw new InvalidOperationException("RPC response result was null.")
            : new MoneroAccount(result.AccountIndex, result.Address);
    }

    public async Task<List<MoneroSubaddress>> GetSubaddresses(uint accountIdx,
        bool skipBalances, List<uint>? subaddressIndices)
    {
        // fetch subaddresses
        MoneroJsonRpcParams parameters = [];
        parameters.Add("account_index", accountIdx);
        if (subaddressIndices != null && subaddressIndices.Count > 0)
        {
            parameters.Add("address_index", subaddressIndices);
        }

        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, GetAddressResult>("get_address", parameters);

        // initialize subaddresses
        List<MoneroSubaddress> subaddresses = [];
        foreach (RpcBalanceInfo rpcSubaddress in result.Addresses)
        {
            MoneroSubaddress subaddress = ConvertRpcSubaddress(rpcSubaddress);
            subaddress.AccountIndex = accountIdx;
            subaddresses.Add(subaddress);
        }

        // fetch and initialize subaddress balances
        if (skipBalances != true)
        {
            // these fields are not initialized if the subaddress is unused and therefore not returned from `get_balance`
            foreach (MoneroSubaddress subaddress in subaddresses)
            {
                subaddress.SetBalance(0);
                subaddress.SetUnlockedBalance(0);
                subaddress.SetNumUnspentOutputs(0);
                subaddress.SetNumBlocksToUnlock(0);
            }

            // fetch and initialize balances
            var getBalanceResult = await _rpc.SendCommandAsync<MoneroJsonRpcParams, GetBalanceResult>("get_balance", parameters);
            foreach (RpcBalanceInfo rpcSubaddress in
                     getBalanceResult.PerSubaddress)
            {
                MoneroSubaddress subaddress = ConvertRpcSubaddress(rpcSubaddress);

                // transfer info to an existing subaddress object
                foreach (MoneroSubaddress tgtSubaddress in subaddresses)
                {
                    if (tgtSubaddress.Index != subaddress.Index)
                    {
                        continue; // skip to subaddress with same index
                    }

                    InitSubaddress(tgtSubaddress, subaddress);
                }
            }
        }

        // cache addresses
        Dictionary<uint, string?>? subaddressMap = _addressCache.GetValueOrDefault(accountIdx);
        if (subaddressMap == null)
        {
            subaddressMap = [];
            _addressCache.Add(accountIdx, subaddressMap);
        }

        foreach (MoneroSubaddress subaddress in subaddresses)
        {
            subaddressMap[(uint)subaddress.Index!] = subaddress.GetAddress();
        }

        // return results
        return subaddresses;
    }

    public async Task<MoneroSubaddress> CreateSubaddress(uint accountIdx, string? label)
    {
        // send request
        MoneroJsonRpcParams parameters = [];
        parameters.Add("account_index", accountIdx);
        parameters.Add("label", label);
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, CreateAddressResult>("create_address", parameters);

        // build subaddress object
        MoneroSubaddress subaddress = new() { AccountIndex = accountIdx, Index = Convert.ToUInt32(result.Index) };
        subaddress.SetAddress((string?)result.Address);
        subaddress.SetLabel(string.IsNullOrEmpty(label) ? "" : label);
        subaddress.SetBalance(0);
        subaddress.SetUnlockedBalance(0);
        subaddress.SetNumUnspentOutputs(0);
        subaddress.SetIsUsed(false);
        subaddress.SetNumBlocksToUnlock(0);
        return subaddress;
    }

    public async Task SetAccountLabel(uint accountIdx, uint subaddressIdx, string label)
    {
        MoneroJsonRpcParams parameters = [];
        Dictionary<string, uint> idx = [];
        idx.Add("major", accountIdx);
        idx.Add("minor", subaddressIdx);
        parameters.Add("index", idx);
        parameters.Add("label", label);
        await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("label_address", parameters);
    }

    public async Task<string> ExportOutputs(bool all)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("all", all);
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, ImportExportOutputsResult>("export_outputs", parameters);
        return result.Hex;
    }

    public async Task<int> ImportOutputs(string outputsHex)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("outputs_data_hex", outputsHex);
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, ImportExportOutputsResult>("import_outputs", parameters);
        return result.NumImported;
    }

    public async Task<List<MoneroKeyImage>> ExportKeyImages(bool all)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("all", all);
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, ExportKeyImagesResult>("export_key_images", parameters);
        return result.SignedKeyImages;
    }

    public async Task<MoneroKeyImageImportResult> ImportKeyImages(List<MoneroKeyImage> keyImages)
    {
        // convert key images to rpc parameter
        List<Dictionary<string, object?>> rpcKeyImages = [];
        foreach (MoneroKeyImage keyImage in keyImages)
        {
            Dictionary<string, object?> rpcKeyImage = [];
            rpcKeyImage.Add("key_image", keyImage.Hex);
            rpcKeyImage.Add("signature", keyImage.Signature);
            rpcKeyImages.Add(rpcKeyImage);
        }

        // send rpc request
        MoneroJsonRpcParams parameters = [];
        parameters.Add("signed_key_images", rpcKeyImages);
        return await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroKeyImageImportResult>("import_key_images", parameters);
    }

    public async Task FreezeOutput(string keyImage)
    {
        if (keyImage == null)
        {
            throw new MoneroError("Must specify key image to freeze");
        }

        MoneroJsonRpcParams parameters = [];
        parameters.Add("key_image", keyImage);
        await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("freeze", parameters);
    }

    public async Task ThawOutput(string keyImage)
    {
        if (keyImage == null)
        {
            throw new MoneroError("Must specify key image to thaw");
        }

        MoneroJsonRpcParams parameters = [];
        parameters.Add("key_image", keyImage);
        await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("thaw", parameters);
    }

    public async Task<bool> IsOutputFrozen(string keyImage)
    {
        if (keyImage == null)
        {
            throw new MoneroError("Must specify key image to check if frozen");
        }

        MoneroJsonRpcParams parameters = [];
        parameters.Add("key_image", keyImage);
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, FrozenResult>("frozen", parameters);
        return result.Frozen;
    }

    public async Task<MoneroTxPriority> GetDefaultFeePriority()
    {
        var response = await _rpc.SendCommandAsync<NoRequestModel, GetDefaultFeePriorityResult>("get_default_fee_priority", NoRequestModel.Instance);
        return (MoneroTxPriority)response.Priority;
    }

    public async Task<string> RelayTx(string txMetadata)
    {
        if (txMetadata == null)
        {
            throw new MoneroError("Must provide an array of tx metadata to relay");
        }

        MoneroJsonRpcParams parameters = [];
        parameters.Add("hex", txMetadata);
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, RelayTxResult>("relay_tx", parameters);

        await Poll(); // notify of changes
        return result.TxHash;
    }

    public async Task<List<string>> SubmitTxs(string signedTxHex)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("tx_data_hex", signedTxHex);
        var resp = await _rpc.SendCommandAsync<MoneroJsonRpcParams, SubmitTransferResult>("submit_transfer", parameters);
        await Poll();
        return resp.TxHashList;
    }

    public async Task<string> SignMessage(string msg, MoneroMessageSignatureType signatureType,
        uint accountIdx,
        uint subaddressIdx)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("data", msg);
        parameters.Add("signature_type",
            signatureType == MoneroMessageSignatureType.SignWithSpendKey ? "spend" : "view");
        parameters.Add("account_index", accountIdx);
        parameters.Add("address_index", subaddressIdx);
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, SignatureResult>("sign", parameters);
        return result.Signature;
    }

    public async Task<MoneroMessageSignatureResult> VerifyMessage(string msg, string address, string signature)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("data", msg);
        parameters.Add("address", address);
        parameters.Add("signature", signature);
        try
        {
            return await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroMessageSignatureResult>("verify", parameters);
        }
        catch (MoneroRpcError e)
        {
            if (-2 == e.GetCode())
            {
                return new MoneroMessageSignatureResult();
            }

            throw;
        }
    }

    public async Task<string> GetTxKey(string txHash)
    {
        try
        {
            MoneroJsonRpcParams parameters = [];
            parameters.Add("txid", txHash);
            var resp = await _rpc.SendCommandAsync<MoneroJsonRpcParams, GetTxKeyResult>("get_tx_key", parameters);
            return resp.TxKey;
        }
        catch (MoneroRpcError e)
        {
            HandleInvalidTxIdError(e);
        }

        throw new MoneroError("Could not get tx key due an unknown error");
    }

    public async Task<MoneroCheckTx> CheckTxKey(string txHash, string txKey, string address)
    {
        try
        {
            // send request
            MoneroJsonRpcParams parameters = [];
            parameters.Add("txid", txHash);
            parameters.Add("tx_key", txKey);
            parameters.Add("address", address);
            return await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroCheckTx>("check_tx_key", parameters);
        }
        catch (MoneroRpcError e)
        {
            HandleInvalidTxIdError(e);
        }

        throw new MoneroError("Could not check tx key due an unknown error");
    }

    public async Task<string> GetTxProof(string txHash, string address, string? message)
    {
        try
        {
            MoneroJsonRpcParams parameters = [];
            parameters.Add("txid", txHash);
            parameters.Add("address", address);
            parameters.Add("message", message);
            var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, SignatureResult>("get_tx_proof", parameters);
            return result.Signature;
        }
        catch (MoneroRpcError e)
        {
            HandleInvalidTxIdError(e);
        }

        throw new MoneroError("Could not get tx proof due an unknown error");
    }

    public async Task<MoneroCheckTx> CheckTxProof(string txHash, string address, string message,
        string signature)
    {
        try
        {
            // send request
            MoneroJsonRpcParams parameters = [];
            parameters.Add("txid", txHash);
            parameters.Add("address", address);
            parameters.Add("message", message);
            parameters.Add("signature", signature);
            return await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroCheckTx>("check_tx_proof", parameters);
        }
        catch (MoneroRpcError e)
        {
            if (-1 == e.GetCode() && e.Message.Equals("basic_string"))
            {
                e = new MoneroRpcError("Must provide signature to check tx proof", -1);
            }

            HandleInvalidTxIdError(e);
        }

        throw new MoneroError("Could not check tx proof due an unknown error");
    }

    public async Task<string> GetSpendProof(string txHash, string? message)
    {
        try
        {
            MoneroJsonRpcParams parameters = [];
            parameters.Add("txid", txHash);
            parameters.Add("message", message);
            var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, SignatureResult>("get_spend_proof", parameters);
            return result.Signature;
        }
        catch (MoneroRpcError e)
        {
            HandleInvalidTxIdError(e);
        }

        throw new MoneroError("Could not get spend proof due an unknown error");
    }

    public async Task<bool> CheckSpendProof(string txHash, string message, string signature)
    {
        try
        {
            MoneroJsonRpcParams parameters = [];
            parameters.Add("txid", txHash);
            parameters.Add("message", message);
            parameters.Add("signature", signature);
            var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, CheckSpendProofResult>("check_spend_proof", parameters);
            return result.Good;
        }
        catch (MoneroRpcError e)
        {
            HandleInvalidTxIdError(e);
        }

        return false;
    }

    public async Task<string> GetReserveProofWallet(string message)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("all", true);
        parameters.Add("message", message);
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, SignatureResult>("get_reserve_proof", parameters);
        return result.Signature;
    }

    public async Task<string> GetReserveProofAccount(uint accountIdx, ulong amount, string message)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("account_index", accountIdx);
        parameters.Add("amount", amount.ToString());
        parameters.Add("message", message);
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, SignatureResult>("get_reserve_proof", parameters);
        return result.Signature;
    }

    public async Task<MoneroCheckReserve> CheckReserveProof(string address, string message, string signature)
    {
        // send request
        MoneroJsonRpcParams parameters = [];
        parameters.Add("address", address);
        parameters.Add("message", message);
        parameters.Add("signature", signature);
        var resp = await _rpc.SendJsonRequest<MoneroCheckReserve>("check_reserve_proof", parameters);
        return resp.Result!;
    }

    public async Task<List<string>> GetTxNotes(List<string> txHashes)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("txids", txHashes);
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, GetTxNotesResult>("get_tx_notes", parameters);
        return result.Notes;
    }

    public async Task SetTxNotes(List<string> txHashes, List<string> notes)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("txids", txHashes);
        parameters.Add("notes", notes);
        await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("set_tx_notes", parameters);
    }

    public async Task<List<MoneroAddressBookEntry>> GetAddressBookEntries(List<uint>? entryIndices)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("entries", entryIndices);
        var resultMap = await _rpc.SendCommandAsync<MoneroJsonRpcParams, GetAddressBookResult>("get_address_book", parameters);
        return resultMap.Entries;
    }

    public async Task<int> AddAddressBookEntry(string address, string description)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("address", address);
        parameters.Add("description", description);
        var resultMap = await _rpc.SendCommandAsync<MoneroJsonRpcParams, SetAddressBookResult>("add_address_book", parameters);
        return resultMap.Index;
    }

    public async Task EditAddressBookEntry(uint index, bool setAddress, string address, bool setDescription,
        string description)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("index", index);
        parameters.Add("set_address", setAddress);
        parameters.Add("address", address);
        parameters.Add("set_description", setDescription);
        parameters.Add("description", description);
        await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("edit_address_book", parameters);
    }

    public async Task DeleteAddressBookEntry(uint entryIdx)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("index", entryIdx);
        await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("delete_address_book", parameters);
    }

    public async Task TagAccounts(string tag, List<uint> accountIndices)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("tag", tag);
        parameters.Add("accounts", accountIndices);
        await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("tag_accounts", parameters);
    }

    public async Task UntagAccounts(List<uint> accountIndices)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("accounts", accountIndices);
        await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("untag_accounts", parameters);
    }

    public async Task<List<MoneroAccountTag>> GetAccountTags()
    {
        var responseMap = await _rpc.SendCommandAsync<NoRequestModel, GetAccountTagsResult>("get_account_tags", NoRequestModel.Instance);
        return responseMap.AccountTags;
    }

    public async Task SetAccountTagLabel(string tag, string label)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("tag", tag);
        parameters.Add("description", label);
        await _rpc.SendJsonRequest<MoneroRpcResponse>("set_account_tag_description", parameters);
    }

    public async Task<string> GetPaymentUri(MoneroTxConfig config)
    {
        if (config == null)
        {
            throw new MoneroError("Must provide configuration to create a payment URI");
        }

        MoneroJsonRpcParams parameters = [];
        parameters.Add("address", config.GetDestinations()![0].Address);
        parameters.Add("amount",
            config.GetDestinations()![0].Amount != null
                ? config.GetDestinations()![0].Amount.ToString()
                : null);
        parameters.Add("payment_id", config.GetPaymentId());
        parameters.Add("recipient_name", config.GetRecipientName());
        parameters.Add("tx_description", config.GetNote());
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, MakeUriResult>("make_uri", parameters);
        return result.Uri;
    }

    public async Task<MoneroTxConfig> ParsePaymentUri(string uri)
    {
        if (string.IsNullOrEmpty(uri))
        {
            throw new MoneroError("Must provide URI to parse");
        }

        MoneroJsonRpcParams parameters = [];
        parameters.Add("uri", uri);
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, ParseUriResult>("parse_uri", parameters);
        ParsedUri rpcUri = result.Uri;
        MoneroTxConfig config = new MoneroTxConfig().SetAddress(rpcUri.Address)
            .SetAmount(rpcUri.Amount);
        config.SetPaymentId(rpcUri.PaymentId);
        config.SetRecipientName(rpcUri.RecipientName);
        config.SetNote(rpcUri.TxDescription);
        if ("".Equals(config.GetDestinations()![0].Address))
        {
            config.GetDestinations()![0].Address = null;
        }

        if ("".Equals(config.GetPaymentId()))
        {
            config.SetPaymentId(null);
        }

        if ("".Equals(config.GetRecipientName()))
        {
            config.SetRecipientName(null);
        }

        if ("".Equals(config.GetNote()))
        {
            config.SetNote(null);
        }

        return config;
    }

    public async Task<string?> GetAttribute(string key)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("key", key);
        try
        {
            var resp = await _rpc.SendJsonRequest<GetAttributeResult>("get_attribute", parameters);
            GetAttributeResult result = resp.Result!;
            return result.Value;
        }
        catch (MoneroRpcError e)
        {
            if (-45 == e.GetCode())
            {
                return null; // -45: attribute not found
            }

            throw;
        }
    }

    public async Task SetAttribute(string key, string val)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("key", key);
        parameters.Add("value", val);
        await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("set_attribute", parameters);
    }

    public async Task StartMining(ulong numThreads, bool backgroundMining, bool ignoreBattery)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("threads_count", numThreads);
        parameters.Add("do_background_mining", backgroundMining);
        parameters.Add("ignore_battery", ignoreBattery);
        await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("start_mining", parameters);
    }

    public async Task StopMining()
    {
        await _rpc.SendCommandAsync<NoRequestModel, MoneroRpcResponse>("stop_mining", NoRequestModel.Instance);
    }

    public async Task<bool> IsMultisigImportNeeded()
    {
        var response = await _rpc.SendCommandAsync<NoRequestModel, GetBalanceResult>("get_balance", NoRequestModel.Instance);
        return response.MultisigImportNeeded;
    }

    public async Task<MoneroMultisigInfo> GetMultisigInfo()
    {
        return await _rpc.SendCommandAsync<NoRequestModel, MoneroMultisigInfo>("is_multisig", NoRequestModel.Instance);
    }

    public async Task<string> PrepareMultisig()
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("enable_multisig_experimental", true);
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, MultisigInfoResult>("prepare_multisig", parameters);
        _addressCache.Clear();
        return result.Info;
    }

    public async Task<string> MakeMultisig(List<string> multisigHexes, int threshold, string password)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("multisig_info", multisigHexes);
        parameters.Add("threshold", threshold);
        parameters.Add("password", password);
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, MultisigInfoResult>("make_multisig", parameters);
        _addressCache.Clear();
        return result.Info;
    }

    public async Task<MoneroMultisigInitResult> ExchangeMultisigKeys(List<string> multisigHexes,
        string password)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("multisig_info", multisigHexes);
        parameters.Add("password", password);
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, MultisigInfoResult>("exchange_multisig_keys", parameters);
        _addressCache.Clear();
        MoneroMultisigInitResult msResult = new() { Address = result.Address, MultisigHex = result.Info };
        if (string.IsNullOrEmpty(msResult.Address))
        {
            msResult.Address = null;
        }

        if (string.IsNullOrEmpty(msResult.MultisigHex))
        {
            msResult.MultisigHex = null;
        }

        return msResult;
    }

    public async Task<string> ExportMultisigHex()
    {
        var response = await _rpc.SendCommandAsync<NoRequestModel, MultisigInfoResult>("export_multisig_info", NoRequestModel.Instance);
        return response.Hex;
    }

    public async Task<int> ImportMultisigHex(List<string> multisigHexes)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("info", multisigHexes);
        var response = await _rpc.SendCommandAsync<MoneroJsonRpcParams, MultisigInfoResult>("import_multisig_info", parameters);
        return response.Outputs;
    }

    public async Task<MoneroMultisigSignResult> SignMultisigTxHex(string multisigTxHex)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("tx_data_hex", multisigTxHex);
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, MultisigInfoResult>("sign_multisig", parameters);

        if (result == null)
        {
            throw new MoneroError("Invalid response from sign_multisig: " + result);
        }

        MoneroMultisigSignResult signResult = new()
        {
            SignedMultisigTxHex = result.TxDataHex,
            TxHashes = result.TxHashList
        };
        return signResult;
    }

    public async Task<List<string>> SubmitMultisigTxHex(string signedMultisigTxHex)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("tx_data_hex", signedMultisigTxHex);
        var resp = await _rpc.SendCommandAsync<MoneroJsonRpcParams, MultisigInfoResult>("submit_multisig", parameters);
        if (resp == null)
        {
            throw new MoneroError("Invalid response from submit_multisig: " + resp);
        }

        return resp.TxHashList;
    }

    public async Task ChangePassword(string oldPassword, string newPassword)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("old_password", oldPassword);
        parameters.Add("new_password", newPassword);
        await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("change_wallet_password", parameters);
    }

    public async Task Save()
    {
        await _rpc.SendCommandAsync<NoRequestModel, MoneroRpcResponse>("store", NoRequestModel.Instance);
    }

    public async Task Close(bool save)
    {
        Clear();
        MoneroJsonRpcParams parameters = [];
        parameters.Add("autosave_current", save);
        await _rpc.SendCommandAsync<MoneroJsonRpcParams, MoneroRpcResponse>("close_wallet", parameters);
    }

    public async Task<bool> IsClosed()
    {
        try
        {
            await GetPrimaryAddress();
        }
        catch (MoneroRpcError e)
        {
            return e.GetCode() == -8 && e.Message.Contains("No wallet file");
        }
        catch
        {
            return false;
        }

        return false;
    }

    #endregion

    #region Private Methods

    private void Clear()
    {
        _listeners.Clear();
        RefreshListening();
        _addressCache.Clear();
        _path = null;
    }

    private void RefreshListening()
    {
        if (_walletPoller == null && _listeners.Count > 0)
        {
            _walletPoller = new MoneroWalletPoller(this, _syncPeriodInMs);
        }

        _walletPoller?.SetIsPolling(_listeners.Count > 0);
    }

    private async Task Poll()
    {
        if (_walletPoller != null && _walletPoller.IsPolling())
        {
            await _walletPoller.Poll();
        }
    }

    private async Task<List<ulong>> GetBalances(uint? accountIdx, uint? subaddressIdx)
    {
        if (accountIdx == null)
        {
            if (subaddressIdx != null)
            {
                throw new MoneroError("Must provide account index with subaddress index");
            }

            ulong balance = 0;
            ulong unlockedBalance = 0;
            foreach (MoneroAccount account in await GetAccounts(false, false, null))
            {
                balance += account.Balance;
                unlockedBalance += account.UnlockedBalance;
            }

            return [balance, unlockedBalance];
        }

        MoneroJsonRpcParams parameters = [];
        parameters.Add("account_index", accountIdx);
        parameters.Add("address_indices", subaddressIdx == null ? null : new List<uint?> { subaddressIdx });
        var result = await _rpc.SendCommandAsync<MoneroJsonRpcParams, GetBalanceResult>("get_balance", parameters);
        if (subaddressIdx == null)
        {
            return [result.Balance, result.UnlockedBalance];
        }

        List<RpcBalanceInfo> rpcBalancesPerSubaddress = result.PerSubaddress;
        return
        [
            Convert.ToUInt64(rpcBalancesPerSubaddress[0].Balance),
            Convert.ToUInt64(rpcBalancesPerSubaddress[0].UnlockedBalance)
        ];
    }

    public async Task<GetTransfersResult> GetTransfers(MoneroTransferQuery? query)
    {
        // copy and normalize query up to block
        if (query == null)
        {
            query = new MoneroTransferQuery();
        }

        // build params for get_transfers rpc call
        MoneroJsonRpcParams parameters = [];
        bool canBeConfirmed = query.IsConfirmed() != false && query.InTxPool() != true &&
                              query.IsFailed() != true && query.IsRelayed() != false;
        bool canBeInTxPool = query.IsConfirmed() != true && query.InTxPool() != false &&
                             query.IsFailed() != true && query.GetHeight() == null &&
                             query.GetMaxHeight() == null && query.IsLocked() != false;
        bool canBeIncoming = query.IsIncoming() != false && query.IsOutgoing() != true &&
                             query.HasDestinations() != true;
        bool canBeOutgoing = query.IsOutgoing() != false && query.IsIncoming() != true;

        // check if fetching pool txs contradicted by configuration
        if (query.InTxPool() == true && !canBeInTxPool)
        {
            throw new MoneroError("Cannot fetch pool transactions because it contradicts configuration");
        }

        parameters.Add("in", canBeIncoming && canBeConfirmed);
        parameters.Add("out", canBeOutgoing && canBeConfirmed);
        parameters.Add("pool", canBeIncoming && canBeInTxPool);
        parameters.Add("pending", canBeOutgoing && canBeInTxPool);
        parameters.Add("failed",
            query.IsFailed() != false && query.IsConfirmed() != true && query.InTxPool() != true);
        if (query.GetMinHeight() != null)
        {
            if (query.GetMinHeight() > 0)
            {
                parameters.Add("min_height",
                    query.GetMinHeight() -
                    1); // TODO monero-project: wallet2::get_payments() min_height is exclusive, so manually offset to match intended range (issues #5751, #5598)
            }
            else
            {
                parameters.Add("min_height", query.GetMinHeight());
            }
        }

        if (query.GetMaxHeight() != null)
        {
            parameters.Add("max_height", query.GetMaxHeight());
        }

        parameters.Add("filter_by_height", query.GetMinHeight() != null || query.GetMaxHeight() != null);
        if (query.GetAccountIndex() == null)
        {
            if (!(query.GetSubaddressIndex() == null && query.GetSubaddressIndices() == null))
            {
                throw new MoneroError("Filter specifies a subaddress index but not an account index");
            }

            parameters.Add("all_accounts", true);
        }
        else
        {
            parameters.Add("account_index", query.GetAccountIndex());

            // set subaddress indices param
            HashSet<uint> subaddressIndices = [];
            if (query.GetSubaddressIndex() != null)
            {
                subaddressIndices.Add((uint)query.GetSubaddressIndex()!);
            }

            if (query.GetSubaddressIndices() != null)
            {
                foreach (uint subaddressIdx in query.GetSubaddressIndices()!)
                {
                    subaddressIndices.Add(subaddressIdx);
                }
            }

            if (subaddressIndices.Count > 0)
            {
                parameters.Add("subaddr_indices", subaddressIndices);
            }
        }

        // build txs using `get_transfers`
        return await _rpc.SendCommandAsync<MoneroJsonRpcParams, GetTransfersResult>("get_transfers", parameters);
    }

    public async Task<GetTransferByTxIdResult> GetTransferByTxId(string txId, int? accountIndex)
    {
        MoneroJsonRpcParams parameters = [];
        parameters.Add("txid", txId);
        parameters.Add("account_index", accountIndex);
        return await _rpc.SendCommandAsync<MoneroJsonRpcParams, GetTransferByTxIdResult>("get_transfer_by_txid", parameters);
    }

    #endregion

    #region Private Static

    private static MoneroSubaddress ConvertRpcSubaddress(RpcBalanceInfo rpcSubaddress)
    {
        MoneroSubaddress subaddress = new()
        {
            AccountIndex = rpcSubaddress.AccountIndex,
            Index = rpcSubaddress.AddressIndex
        };

        subaddress.SetAddress(rpcSubaddress.Address);
        subaddress.SetBalance(rpcSubaddress.Balance);
        subaddress.SetUnlockedBalance(rpcSubaddress.UnlockedBalance);
        subaddress.SetNumUnspentOutputs(rpcSubaddress.NumUnspentOutputs);
        subaddress.SetLabel(rpcSubaddress.Label);
        subaddress.SetIsUsed(rpcSubaddress.Used);
        subaddress.SetNumBlocksToUnlock(rpcSubaddress.BlocksToUnlock);

        return subaddress;
    }

    public async Task<string> GetPrimaryAddress()
    {
        string? address = await GetAddress(0, 0);

        if (address == null)
        {
            throw new MoneroError("Could not get primary address");
        }

        return address;
    }

    public async Task<bool> IsMultisig()
    {
        MoneroMultisigInfo info = await GetMultisigInfo();
        return info.IsMultisig == true;
    }

    public Task<MoneroNetworkType> GetNetworkType()
    {
        throw new NotImplementedException();
    }

    public async Task<MoneroSubaddress> GetSubaddress(uint accountIdx, uint subaddressIdx)
    {
        List<MoneroSubaddress> subaddresses = await GetSubaddresses(accountIdx, false, [subaddressIdx]);
        if (subaddresses.Count == 0)
        {
            throw new MoneroError("Subaddress at index " + subaddressIdx + " is not initialized");
        }

        if (1 != subaddresses.Count)
        {
            throw new MoneroError("Only 1 subaddress should be returned");
        }

        return subaddresses[0];
    }

    #endregion
}