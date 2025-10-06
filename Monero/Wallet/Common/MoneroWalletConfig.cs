using Monero.Common;

namespace Monero.Wallet.Common;

public class MoneroWalletConfig
{
    private int? _accountLookahead; // number of accounts to scan
    private byte[]? _cacheData;
    private bool? _isMultisig;
    private byte[]? _keysData;
    private string? _language;
    private MoneroNetworkType? _networkType;
    private string? _password;
    private string? _path;
    private string? _primaryAddress;
    private string? _privateSpendKey;
    private string? _privateViewKey;
    private ulong? _restoreHeight;
    private bool? _saveCurrent;
    private string? _seed;
    private string? _seedOffset;
    private MoneroRpcConnection? _server;
    private string? _serverPassword;
    private string? _serverUsername;
    private int? _subaddressLookahead; // number of subaddresses to scan per account

    public MoneroWalletConfig() { }

    public MoneroWalletConfig(MoneroWalletConfig config)
    {
        _path = config.GetPath();
        _password = config.GetPassword();
        _networkType = config.GetNetworkType();
        _server = config.GetServer();
        _seed = config.GetSeed();
        _seedOffset = config.GetSeedOffset();
        _primaryAddress = config.GetPrimaryAddress();
        _privateViewKey = config.GetPrivateViewKey();
        _privateSpendKey = config.GetPrivateSpendKey();
        _restoreHeight = config.GetRestoreHeight();
        _language = config.GetLanguage();
        _saveCurrent = config.GetSaveCurrent();
        _accountLookahead = config.GetAccountLookahead();
        _subaddressLookahead = config.GetSubaddressLookahead();
        _keysData = config.GetKeysData();
        _cacheData = config.GetCacheData();
        _isMultisig = config.IsMultisig();
    }

    public MoneroWalletConfig Clone()
    {
        return new MoneroWalletConfig(this);
    }

    public string? GetPath()
    {
        return _path;
    }

    public MoneroWalletConfig SetPath(string? path)
    {
        _path = path;
        return this;
    }

    public string? GetPassword()
    {
        return _password;
    }

    public MoneroWalletConfig SetPassword(string? password)
    {
        _password = password;
        return this;
    }

    public MoneroNetworkType? GetNetworkType()
    {
        return _networkType;
    }

    public MoneroWalletConfig SetNetworkType(MoneroNetworkType networkType)
    {
        _networkType = networkType;
        return this;
    }

    public MoneroWalletConfig SetNetworkType(string? networkTypeStr)
    {
        return SetNetworkType(MoneroNetwork.Parse(networkTypeStr));
    }

    public MoneroRpcConnection? GetServer()
    {
        return _server;
    }

    public MoneroWalletConfig SetServer(MoneroRpcConnection? server)
    {
        _server = server;
        _serverUsername = server == null ? null : server.GetUsername();
        _serverPassword = server == null ? null : server.GetPassword();
        return this;
    }

    public string? GetServerUri()
    {
        return _server?.GetUri();
    }

    public string? GetServerUsername()
    {
        return _server?.GetUsername();
    }

    public MoneroWalletConfig SetServerUsername(string? serverUsernameInput)
    {
        _serverUsername = serverUsernameInput;
        if (_server != null && serverUsernameInput != null && _serverPassword != null)
        {
            _server.SetCredentials(serverUsernameInput, _serverPassword);
        }

        return this;
    }

    public string? GetServerPassword()
    {
        return _server?.GetPassword();
    }

    public MoneroWalletConfig SetServerPassword(string? serverPassword)
    {
        _serverPassword = serverPassword;
        if (_server != null && _serverUsername != null && serverPassword != null)
        {
            _server.SetCredentials(_serverUsername, serverPassword);
        }

        return this;
    }

    public string? GetSeed()
    {
        return _seed;
    }

    public MoneroWalletConfig SetSeed(string? seed)
    {
        _seed = seed;
        return this;
    }

    public string? GetSeedOffset()
    {
        return _seedOffset;
    }

    public MoneroWalletConfig SetSeedOffset(string? seedOffset)
    {
        _seedOffset = seedOffset;
        return this;
    }

    public string? GetPrimaryAddress()
    {
        return _primaryAddress;
    }

    public MoneroWalletConfig SetPrimaryAddress(string? primaryAddress)
    {
        _primaryAddress = primaryAddress;
        return this;
    }

    public string? GetPrivateViewKey()
    {
        return _privateViewKey;
    }

    public MoneroWalletConfig SetPrivateViewKey(string? privateViewKey)
    {
        _privateViewKey = privateViewKey;
        return this;
    }

    public string? GetPrivateSpendKey()
    {
        return _privateSpendKey;
    }

    public MoneroWalletConfig SetPrivateSpendKey(string? privateSpendKey)
    {
        _privateSpendKey = privateSpendKey;
        return this;
    }

    public ulong? GetRestoreHeight()
    {
        return _restoreHeight;
    }

    public MoneroWalletConfig SetRestoreHeight(ulong? restoreHeight)
    {
        _restoreHeight = restoreHeight;
        return this;
    }

    public string? GetLanguage()
    {
        return _language;
    }

    public MoneroWalletConfig SetLanguage(string? language)
    {
        _language = language;
        return this;
    }

    public bool? GetSaveCurrent()
    {
        return _saveCurrent;
    }

    public MoneroWalletConfig SetSaveCurrent(bool? saveCurrent)
    {
        _saveCurrent = saveCurrent;
        return this;
    }

    public MoneroWalletConfig SetAccountLookahead(int? accountLookahead)
    {
        _accountLookahead = accountLookahead;
        return this;
    }

    public int? GetAccountLookahead()
    {
        return _accountLookahead;
    }

    public MoneroWalletConfig SetSubaddressLookahead(int? subaddressLookahead)
    {
        _subaddressLookahead = subaddressLookahead;
        return this;
    }

    public int? GetSubaddressLookahead()
    {
        return _subaddressLookahead;
    }

    public byte[]? GetKeysData()
    {
        return _keysData;
    }

    public MoneroWalletConfig SetKeysData(byte[]? keysData)
    {
        _keysData = keysData;
        return this;
    }

    public byte[]? GetCacheData()
    {
        return _cacheData;
    }

    public MoneroWalletConfig SetCacheData(byte[]? cacheData)
    {
        _cacheData = cacheData;
        return this;
    }

    public bool? IsMultisig()
    {
        return _isMultisig;
    }

    public MoneroWalletConfig SetIsMultisig(bool? isMultisig)
    {
        _isMultisig = isMultisig;
        return this;
    }
}