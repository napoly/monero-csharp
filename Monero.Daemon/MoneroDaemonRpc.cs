using Monero.Common;
using Monero.Daemon.Common;

namespace Monero.Daemon
{
    public class MoneroDaemonRpc : MoneroDaemonDefault
    {
        public override void AddListener(MoneroDaemonListener listener)
        {
            throw new NotImplementedException();
        }

        public override MoneroDaemonUpdateCheckResult CheckForUpdate()
        {
            throw new NotImplementedException();
        }

        public override MoneroDaemonUpdateDownloadResult DownloadUpdate()
        {
            throw new NotImplementedException();
        }

        public override MoneroDaemonUpdateDownloadResult DownloadUpdate(string path)
        {
            throw new NotImplementedException();
        }

        public override void FlushTxPool()
        {
            throw new NotImplementedException();
        }

        public override void FlushTxPool(List<string> txHashes)
        {
            throw new NotImplementedException();
        }

        public override List<string> GetAltBlockHashes()
        {
            throw new NotImplementedException();
        }

        public override List<MoneroAltChain> GetAltChains()
        {
            throw new NotImplementedException();
        }

        public override MoneroBlock GetBlockByHash(string blockHash)
        {
            throw new NotImplementedException();
        }

        public override MoneroBlock GetBlockByHeight(long blockHeight)
        {
            throw new NotImplementedException();
        }

        public override string GetBlockHash()
        {
            throw new NotImplementedException();
        }

        public override List<string> GetBlockHashes(List<string> blockHashes, long startHeight)
        {
            throw new NotImplementedException();
        }

        public override MoneroBlockHeader GetBlockHeaderByHash(string blockHash)
        {
            throw new NotImplementedException();
        }

        public override MoneroBlockHeader GetBlockHeaderByHeight(long blockHeight)
        {
            throw new NotImplementedException();
        }

        public override List<MoneroBlockHeader> GetBlockHeadersByRange(long startHeight, long endHeight)
        {
            throw new NotImplementedException();
        }

        public override List<MoneroBlock> GetBlocksByHash(List<string> blockHashes, long startHeight, bool prune)
        {
            throw new NotImplementedException();
        }

        public override List<MoneroBlock> GetBlocksByHeight(List<long> blockHeights)
        {
            throw new NotImplementedException();
        }

        public override List<MoneroBlock> GetBlocksByRange(long startHeight, long endHeight)
        {
            throw new NotImplementedException();
        }

        public override List<MoneroBlock> GetBlocksByRangeChunked(long startHeight, long endHeight)
        {
            throw new NotImplementedException();
        }

        public override MoneroBlockTemplate GetBlockTemplate(string walletAddress, int? reserveSize = null)
        {
            throw new NotImplementedException();
        }

        public override int GetDownloadLimit()
        {
            throw new NotImplementedException();
        }

        public override MoneroFeeEstimate GetFeeEstimate(int? graceBlocks = null)
        {
            throw new NotImplementedException();
        }

        public override MoneroHardForkInfo GetHardForkInfo()
        {
            throw new NotImplementedException();
        }

        public override long GetHeight()
        {
            throw new NotImplementedException();
        }

        public override MoneroDaemonInfo GetInfo()
        {
            throw new NotImplementedException();
        }

        public override List<MoneroKeyImage.SpentStatus> GetKeyImageSpentStatuses(List<string> keyImage)
        {
            throw new NotImplementedException();
        }

        public override List<MoneroPeer> GetKnownPeers()
        {
            throw new NotImplementedException();
        }

        public override MoneroBlockHeader GetLastBlockHeader()
        {
            throw new NotImplementedException();
        }

        public override List<MoneroDaemonListener> GetListeners()
        {
            throw new NotImplementedException();
        }

        public override MoneroMiningStatus GetMiningStatus()
        {
            throw new NotImplementedException();
        }

        public override List<MoneroOutputDistributionEntry> GetOutputDistribution(List<long> amounts, bool isCumulative, long startHeight, long endHeight)
        {
            throw new NotImplementedException();
        }

        public override List<MoneroOutputHistogramEntry> GetOutputHistogram(List<long> amounts, int minCount, int maxCount, bool isUnlocked, int recentCutoff)
        {
            throw new NotImplementedException();
        }

        public override List<MoneroOutput> GetOutputs(List<MoneroOutput> outputs)
        {
            throw new NotImplementedException();
        }

        public override List<MoneroBan> GetPeerBans()
        {
            throw new NotImplementedException();
        }

        public override List<MoneroPeer> GetPeers()
        {
            throw new NotImplementedException();
        }

        public override MoneroDaemonSyncInfo GetSyncInfo()
        {
            throw new NotImplementedException();
        }

        public override MoneroTx GetTx(string txHash, bool prune = false)
        {
            throw new NotImplementedException();
        }

        public override string GetTxHex(string txHash, bool prune = false)
        {
            throw new NotImplementedException();
        }

        public override List<string> GetTxHexes(List<string> txHashes, bool prune = false)
        {
            throw new NotImplementedException();
        }

        public override List<MoneroTx> GetTxPool()
        {
            throw new NotImplementedException();
        }

        public override List<string> GetTxPoolHashes()
        {
            throw new NotImplementedException();
        }

        public override MoneroTxPoolStats GetTxPoolStats()
        {
            throw new NotImplementedException();
        }

        public override List<MoneroTx> GetTxs(List<string> txHashes, bool prune = false)
        {
            throw new NotImplementedException();
        }

        public override int GetUploadLimit()
        {
            throw new NotImplementedException();
        }

        public override MoneroVersion GetVersion()
        {
            throw new NotImplementedException();
        }

        public override bool IsTrusted()
        {
            throw new NotImplementedException();
        }

        public override MoneroPruneResult PruneBlockchain(bool check)
        {
            throw new NotImplementedException();
        }

        public override void RelayTxsByHash(List<string> txHashes)
        {
            throw new NotImplementedException();
        }

        public override void ResetDownloadLimit()
        {
            throw new NotImplementedException();
        }

        public override void ResetUploadLimit()
        {
            throw new NotImplementedException();
        }

        public override void SetDownloadLimit(int limit)
        {
            throw new NotImplementedException();
        }

        public override void SetIncomingPeerLimit(int limit)
        {
            throw new NotImplementedException();
        }

        public override void SetOutgoingPeerLimit(int limit)
        {
            throw new NotImplementedException();
        }

        public override void SetPeerBans(List<MoneroBan> bans)
        {
            throw new NotImplementedException();
        }

        public override void SetUploadLimit(int limit)
        {
            throw new NotImplementedException();
        }

        public override void StartMining(string address, long numThreads, bool isBackground, bool ignoreBattery)
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public override void StopMining()
        {
            throw new NotImplementedException();
        }

        public override void SubmitBlocks(List<string> blockBlobs)
        {
            throw new NotImplementedException();
        }

        public override MoneroSubmitTxResult SubmitTxHex(string txHex, bool doNotRelay = false)
        {
            throw new NotImplementedException();
        }

        public override MoneroBlockHeader WaitForNextBlockHeader()
        {
            throw new NotImplementedException();
        }
    }
}
