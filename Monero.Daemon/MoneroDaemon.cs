using Monero.Common;
using Monero.Daemon.Common;

namespace Monero.Daemon
{
    public interface MoneroDaemon
    {
        public void AddListener(MoneroDaemonListener listener);
        public void RemoveListener(MoneroDaemonListener listener);
        public List<MoneroDaemonListener> GetListeners();
        public MoneroVersion GetVersion();
        public bool IsTrusted();
        public long GetHeight();
        public string GetBlockHash();
        public MoneroBlockTemplate GetBlockTemplate(string walletAddress, int? reserveSize = null);
        public MoneroBlockHeader GetLastBlockHeader();
        public MoneroBlockHeader GetBlockHeaderByHash(string blockHash);
        public MoneroBlockHeader GetBlockHeaderByHeight(long blockHeight);
        public List<MoneroBlockHeader> GetBlockHeadersByRange(long startHeight, long endHeight);
        public MoneroBlock GetBlockByHash(string blockHash);
        public List<MoneroBlock> GetBlocksByHash(List<string> blockHashes, long startHeight, bool prune);
        public MoneroBlock GetBlockByHeight(long blockHeight);
        public List<MoneroBlock> GetBlocksByHeight(List<long> blockHeights);
        public List<MoneroBlock> GetBlocksByRange(long startHeight, long endHeight);
        public List<MoneroBlock> GetBlocksByRangeChunked(long startHeight, long endHeight);
        public List<string> GetBlockHashes(List<string> blockHashes, long startHeight);
        public MoneroTx GetTx(string txHash, bool prune = false);
        public List<MoneroTx> GetTxs(List<string> txHashes, bool prune = false);
        public string GetTxHex(string txHash, bool prune = false);
        public List<string> GetTxHexes(List<string> txHashes, bool prune = false);
        public MoneroFeeEstimate GetFeeEstimate(int? graceBlocks = null);
        public MoneroSubmitTxResult SubmitTxHex(string txHex, bool doNotRelay = false);
        public void RelayTxByHash(string txHash);
        public void RelayTxsByHash(List<string> txHashes);
        public List<MoneroTx> GetTxPool();
        public List<string> GetTxPoolHashes();
        public MoneroTxPoolStats GetTxPoolStats();
        public void FlushTxPool();
        public void FlushTxPool(List<string> txHashes);
        public MoneroKeyImage.SpentStatus GetKeyImageSpentStatus(string keyImage);
        public List<MoneroKeyImage.SpentStatus> GetKeyImageSpentStatuses(List<string> keyImage);
        public List<MoneroOutput> GetOutputs(List<MoneroOutput> outputs);
        public List<MoneroOutputHistogramEntry> GetOutputHistogram(List<long> amounts, int minCount, int maxCount, bool isUnlocked, int recentCutoff);
        public List<MoneroOutputDistributionEntry> GetOutputDistribution(List<long> amounts, bool isCumulative, long startHeight, long endHeight);
        public MoneroDaemonInfo GetInfo();
        public MoneroDaemonSyncInfo GetSyncInfo();
        public MoneroHardForkInfo GetHardForkInfo();
        public List<MoneroAltChain> GetAltChains();
        public List<string> GetAltBlockHashes();
        public int GetDownloadLimit();
        public void SetDownloadLimit(int limit);
        public void ResetDownloadLimit();
        public int GetUploadLimit();
        public void SetUploadLimit(int limit);
        public void ResetUploadLimit();
        public List<MoneroPeer> GetPeers();
        public List<MoneroPeer> GetKnownPeers();
        public void SetOutgoingPeerLimit(int limit);
        public void SetIncomingPeerLimit(int limit);
        public List<MoneroBan> GetPeerBans();
        public void SetPeerBan(MoneroBan ban);
        public void SetPeerBans(List<MoneroBan> bans);
        public void StartMining(string address, long numThreads, bool isBackground, bool ignoreBattery);
        public void StopMining();
        public MoneroMiningStatus GetMiningStatus();
        public void SubmitBlock(string blockBlob);
        public void SubmitBlocks(List<string> blockBlobs);
        public MoneroPruneResult PruneBlockchain(bool check);
        public MoneroDaemonUpdateCheckResult CheckForUpdate();
        public MoneroDaemonUpdateDownloadResult DownloadUpdate();
        public MoneroDaemonUpdateDownloadResult DownloadUpdate(string path);
        public void Stop();
        public MoneroBlockHeader WaitForNextBlockHeader();
    }
}
