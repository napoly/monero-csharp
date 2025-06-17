
using Monero.Common;
using Monero.Wallet.Common;
using System.Collections.ObjectModel;

namespace Monero.Wallet
{
    public abstract class MoneroWalletDefault : MoneroWallet
    {
        public int AddAddressBookEntry(string address, string description)
        {
            throw new NotImplementedException();
        }

        public void AddListener(MoneroWalletListener listener)
        {
            throw new NotImplementedException();
        }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public MoneroCheckReserve CheckReserveProof(string address, string message, string signature)
        {
            throw new NotImplementedException();
        }

        public bool CheckSpendProof(string txHash, string message, string signature)
        {
            throw new NotImplementedException();
        }

        public MoneroCheckTx CheckTxKey(string txHash, string txKey, string address)
        {
            throw new NotImplementedException();
        }

        public MoneroCheckTx CheckTxProof(string txHash, string address, string message, string signature)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Close(bool save)
        {
            throw new NotImplementedException();
        }

        public MoneroAccount CreateAccount()
        {
            throw new NotImplementedException();
        }

        public MoneroAccount CreateAccount(string label)
        {
            throw new NotImplementedException();
        }

        public MoneroSubaddress CreateSubaddress(int accountIdx)
        {
            throw new NotImplementedException();
        }

        public MoneroSubaddress CreateSubaddress(int accountIdx, string label)
        {
            throw new NotImplementedException();
        }

        public MoneroTxWallet CreateTx(MoneroTxConfig config)
        {
            throw new NotImplementedException();
        }

        public List<MoneroTxWallet> CreateTxs(MoneroTxConfig config)
        {
            throw new NotImplementedException();
        }

        public MoneroIntegratedAddress decodeIntegratedAddress(string integratedAddress)
        {
            throw new NotImplementedException();
        }

        public void DeleteAddressBookEntry(int entryIdx)
        {
            throw new NotImplementedException();
        }

        public MoneroTxSet DescribeMultisigTxSet(string multisigTxHex)
        {
            throw new NotImplementedException();
        }

        public MoneroTxSet DescribeTxSet(MoneroTxSet txSet)
        {
            throw new NotImplementedException();
        }

        public MoneroTxSet DescribeUnsignedTxSet(string unsignedTxHex)
        {
            throw new NotImplementedException();
        }

        public void EditAddressBookEntry(int index, bool setAddress, string address, bool setDescription, string description)
        {
            throw new NotImplementedException();
        }

        public MoneroMultisigInitResult ExchangeMultisigKeys(List<string> multisigHexes, string password)
        {
            throw new NotImplementedException();
        }

        public List<MoneroKeyImage> ExportKeyImages()
        {
            throw new NotImplementedException();
        }

        public List<MoneroKeyImage> ExportKeyImages(bool all)
        {
            throw new NotImplementedException();
        }

        public string ExportMultisigHex()
        {
            throw new NotImplementedException();
        }

        public string ExportOutputs()
        {
            throw new NotImplementedException();
        }

        public string ExportOutputs(bool all)
        {
            throw new NotImplementedException();
        }

        public void FreezeOutput(string keyImage)
        {
            throw new NotImplementedException();
        }

        public MoneroAccount GetAccount(int accountIdx)
        {
            throw new NotImplementedException();
        }

        public MoneroAccount GetAccount(int accountIdx, bool includeSubaddresses)
        {
            throw new NotImplementedException();
        }

        public List<MoneroAccount> GetAccounts()
        {
            throw new NotImplementedException();
        }

        public List<MoneroAccount> GetAccounts(bool includeSubaddresses)
        {
            throw new NotImplementedException();
        }

        public List<MoneroAccount> GetAccounts(string tag)
        {
            throw new NotImplementedException();
        }

        public List<MoneroAccount> GetAccounts(bool includeSubaddresses, string tag)
        {
            throw new NotImplementedException();
        }

        public List<MoneroAccountTag> GetAccountTags()
        {
            throw new NotImplementedException();
        }

        public string GetAddress(int accountIdx, int subaddressIdx)
        {
            throw new NotImplementedException();
        }

        public List<MoneroAddressBookEntry> GetAddressBookEntries()
        {
            throw new NotImplementedException();
        }

        public List<MoneroAddressBookEntry> GetAddressBookEntries(List<uint> entryIndices)
        {
            throw new NotImplementedException();
        }

        public MoneroSubaddress GetAddressIndex(string address)
        {
            throw new NotImplementedException();
        }

        public string GetAttribute(string key)
        {
            throw new NotImplementedException();
        }

        public ulong GetBalance()
        {
            throw new NotImplementedException();
        }

        public ulong GetBalance(uint accountIdx)
        {
            throw new NotImplementedException();
        }

        public ulong GetBalance(uint accountIdx, uint subaddressIdx)
        {
            throw new NotImplementedException();
        }

        public MoneroConnectionManager GetConnectionManager()
        {
            throw new NotImplementedException();
        }

        public MoneroRpcConnection GetDaemonConnection()
        {
            throw new NotImplementedException();
        }

        public long GetDaemonHeight()
        {
            throw new NotImplementedException();
        }

        public MoneroTxPriority GetDefaultFeePriority()
        {
            throw new NotImplementedException();
        }

        public long GetHeight()
        {
            throw new NotImplementedException();
        }

        public long GetHeightByDate(int year, int month, int day)
        {
            throw new NotImplementedException();
        }

        public List<MoneroIncomingTransfer> GetIncomingTransfers()
        {
            throw new NotImplementedException();
        }

        public List<MoneroIncomingTransfer> GetIncomingTransfers(MoneroTransferQuery query)
        {
            throw new NotImplementedException();
        }

        public MoneroIntegratedAddress GetIntegratedAddress()
        {
            throw new NotImplementedException();
        }

        public MoneroIntegratedAddress GetIntegratedAddress(string standardAddress, string paymentId)
        {
            throw new NotImplementedException();
        }

        public List<MoneroWalletListener> GetListeners()
        {
            throw new NotImplementedException();
        }

        public MoneroMultisigInfo GetMultisigInfo()
        {
            throw new NotImplementedException();
        }

        public List<MoneroKeyImage> GetNewKeyImagesFromLastImport()
        {
            throw new NotImplementedException();
        }

        public List<MoneroOutgoingTransfer> GetOutgoingTransfers()
        {
            throw new NotImplementedException();
        }

        public List<MoneroOutgoingTransfer> GetOutgoingTransfers(MoneroTransferQuery query)
        {
            throw new NotImplementedException();
        }

        public List<MoneroOutputWallet> GetOutputs()
        {
            throw new NotImplementedException();
        }

        public List<MoneroOutputWallet> GetOutputs(MoneroOutputQuery query)
        {
            throw new NotImplementedException();
        }

        public string GetPath()
        {
            throw new NotImplementedException();
        }

        public string GetPaymentUri(MoneroTxConfig config)
        {
            throw new NotImplementedException();
        }

        public string GetPrimaryAddress()
        {
            throw new NotImplementedException();
        }

        public string GetPrivateSpendKey()
        {
            throw new NotImplementedException();
        }

        public string GetPrivateViewKey()
        {
            throw new NotImplementedException();
        }

        public string GetPublicSpendKey()
        {
            throw new NotImplementedException();
        }

        public string GetPublicViewKey()
        {
            throw new NotImplementedException();
        }

        public string GetReserveProofAccount(int accountIdx, ulong amount, string message)
        {
            throw new NotImplementedException();
        }

        public string GetReserveProofWallet(string message)
        {
            throw new NotImplementedException();
        }

        public string GetSeed()
        {
            throw new NotImplementedException();
        }

        public string GetSeedLanguage()
        {
            throw new NotImplementedException();
        }

        public string GetSpendProof(string txHash)
        {
            throw new NotImplementedException();
        }

        public string GetSpendProof(string txHash, string message)
        {
            throw new NotImplementedException();
        }

        public MoneroSubaddress GetSubaddress(int accountIdx, int subaddressIdx)
        {
            throw new NotImplementedException();
        }

        public List<MoneroSubaddress> GetSubaddresses(int accountIdx)
        {
            throw new NotImplementedException();
        }

        public List<MoneroSubaddress> GetSubaddresses(int accountIdx, List<uint> subaddressIndices)
        {
            throw new NotImplementedException();
        }

        public List<MoneroTransfer> GetTransfers()
        {
            throw new NotImplementedException();
        }

        public List<MoneroTransfer> GetTransfers(int accountIdx)
        {
            throw new NotImplementedException();
        }

        public List<MoneroTransfer> GetTransfers(int accountIdx, int subaddressIdx)
        {
            throw new NotImplementedException();
        }

        public List<MoneroTransfer> GetTransfers(MoneroTransferQuery query)
        {
            throw new NotImplementedException();
        }

        public MoneroTxWallet GetTx(string txHash)
        {
            throw new NotImplementedException();
        }

        public string GetTxKey(string txHash)
        {
            throw new NotImplementedException();
        }

        public string GetTxNote(string txHash)
        {
            throw new NotImplementedException();
        }

        public List<string> GetTxNotes(List<string> txHashes)
        {
            throw new NotImplementedException();
        }

        public string GetTxProof(string txHash, string address)
        {
            throw new NotImplementedException();
        }

        public string GetTxProof(string txHash, string address, string message)
        {
            throw new NotImplementedException();
        }

        public List<MoneroTxWallet> GetTxs()
        {
            throw new NotImplementedException();
        }

        public List<MoneroTxWallet> GetTxs(List<string> txHashes)
        {
            throw new NotImplementedException();
        }

        public List<MoneroTxWallet> GetTxs(MoneroTxQuery query)
        {
            throw new NotImplementedException();
        }

        public ulong GetUnlockedBalance()
        {
            throw new NotImplementedException();
        }

        public ulong GetUnlockedBalance(uint accountIdx)
        {
            throw new NotImplementedException();
        }

        public ulong GetUnlockedBalance(uint accountIdx, uint subaddressIdx)
        {
            throw new NotImplementedException();
        }

        public MoneroVersion GetVersion()
        {
            throw new NotImplementedException();
        }

        public MoneroKeyImageImportResult ImportKeyImages(List<MoneroKeyImage> keyImages)
        {
            throw new NotImplementedException();
        }

        public int ImportMultisigHex(List<string> multisigHexes)
        {
            throw new NotImplementedException();
        }

        public int ImportOutputs(string outputsHex)
        {
            throw new NotImplementedException();
        }

        public bool IsClosed()
        {
            throw new NotImplementedException();
        }

        public bool IsConnectedToDaemon()
        {
            throw new NotImplementedException();
        }

        public bool IsMultisig()
        {
            throw new NotImplementedException();
        }

        public bool IsMultisigImportNeeded()
        {
            throw new NotImplementedException();
        }

        public bool IsOutputFrozen(string keyImage)
        {
            throw new NotImplementedException();
        }

        public bool IsViewOnly()
        {
            throw new NotImplementedException();
        }

        public string MakeMultisig(List<string> multisigHexes, int threshold, string password)
        {
            throw new NotImplementedException();
        }

        public MoneroTxConfig ParsePaymentUri(string uri)
        {
            throw new NotImplementedException();
        }

        public string PrepareMultisig()
        {
            throw new NotImplementedException();
        }

        public string RelayTx(string txMetadata)
        {
            throw new NotImplementedException();
        }

        public string RelayTx(MoneroTxWallet tx)
        {
            throw new NotImplementedException();
        }

        public List<string> RelayTxs(Collection<string> txMetadatas)
        {
            throw new NotImplementedException();
        }

        public List<string> RelayTxs(List<MoneroTxWallet> txs)
        {
            throw new NotImplementedException();
        }

        public void RemoveListener(MoneroWalletListener listener)
        {
            throw new NotImplementedException();
        }

        public void RescanBlockchain()
        {
            throw new NotImplementedException();
        }

        public void RescanSpent()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool ScanTxs(Collection<string> txHashes)
        {
            throw new NotImplementedException();
        }

        public bool SetAccountLabel(int accountIdx, string label)
        {
            throw new NotImplementedException();
        }

        public bool SetAccountTagLabel(string tag, string label)
        {
            throw new NotImplementedException();
        }

        public bool SetAttribute(string key, string val)
        {
            throw new NotImplementedException();
        }

        public bool SetConnectionManager(MoneroConnectionManager connectionManager)
        {
            throw new NotImplementedException();
        }

        public bool SetDaemonConnection(string uri)
        {
            throw new NotImplementedException();
        }

        public bool SetDaemonConnection(string uri, string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool SetDaemonConnection(MoneroRpcConnection daemonConnection)
        {
            throw new NotImplementedException();
        }

        public bool SetProxyUri(string uri)
        {
            throw new NotImplementedException();
        }

        public bool SetSubaddressLabel(int accountIdx, int subaddressIdx, string label)
        {
            throw new NotImplementedException();
        }

        public bool SetTxNote(string txHash, string note)
        {
            throw new NotImplementedException();
        }

        public bool SetTxNotes(List<string> txHashes, List<string> notes)
        {
            throw new NotImplementedException();
        }

        public string SignMessage(string message)
        {
            throw new NotImplementedException();
        }

        public string SignMessage(string message, MoneroMessageSignatureType signatureType, int accountIdx, int subaddressIdx)
        {
            throw new NotImplementedException();
        }

        public MoneroMultisigSignResult SignMultisigTxHex(string multisigTxHex)
        {
            throw new NotImplementedException();
        }

        public MoneroTxSet SignTxs(string unsignedTxHex)
        {
            throw new NotImplementedException();
        }

        public bool StartMining(ulong numThreads, bool backgroundMining, bool ignoreBattery)
        {
            throw new NotImplementedException();
        }

        public bool StartSyncing()
        {
            throw new NotImplementedException();
        }

        public bool StartSyncing(ulong SyncPeriodInMs)
        {
            throw new NotImplementedException();
        }

        public bool StopMining()
        {
            throw new NotImplementedException();
        }

        public bool StopSyncing()
        {
            throw new NotImplementedException();
        }

        public List<string> SubmitMultisigTxHex(string signedMultisigTxHex)
        {
            throw new NotImplementedException();
        }

        public List<string> SubmitTxs(string signedTxHex)
        {
            throw new NotImplementedException();
        }

        public List<MoneroTxWallet> SweepDust(bool relay)
        {
            throw new NotImplementedException();
        }

        public MoneroTxWallet SweepOutput(MoneroTxConfig config)
        {
            throw new NotImplementedException();
        }

        public List<MoneroTxWallet> SweepUnlocked(MoneroTxConfig config)
        {
            throw new NotImplementedException();
        }

        public MoneroSyncResult Sync()
        {
            throw new NotImplementedException();
        }

        public MoneroSyncResult Sync(MoneroWalletListener listener)
        {
            throw new NotImplementedException();
        }

        public MoneroSyncResult Sync(ulong startHeight)
        {
            throw new NotImplementedException();
        }

        public MoneroSyncResult Sync(ulong startHeight, MoneroWalletListener listener)
        {
            throw new NotImplementedException();
        }

        public void TagAccounts(string tag, Collection<uint> accountIndices)
        {
            throw new NotImplementedException();
        }

        public void ThawOutput(string keyImage)
        {
            throw new NotImplementedException();
        }

        public void UntagAccounts(Collection<uint> accountIndices)
        {
            throw new NotImplementedException();
        }

        public MoneroMessageSignatureResult VerifyMessage(string message, string address, string signature)
        {
            throw new NotImplementedException();
        }
    }
}
