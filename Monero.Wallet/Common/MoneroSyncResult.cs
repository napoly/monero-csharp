
namespace Monero.Wallet.Common
{
    public class MoneroSyncResult
    {
        private long _numBlocksFetched;
        private bool _receivedMoney;

        public MoneroSyncResult(long numBlocksFetched, bool receivedMoney)
        {
            _numBlocksFetched = numBlocksFetched;
            _receivedMoney = receivedMoney;
        }

        public MoneroSyncResult(MoneroSyncResult syncResult)
        {
            _numBlocksFetched = syncResult._numBlocksFetched;
            _receivedMoney = syncResult._receivedMoney;
        }

        public long GetNumBlocksFetched()
        {
            return _numBlocksFetched;
        }

        public MoneroSyncResult SetNumBlocksFetched(long numBlocksFetched)
        {
            _numBlocksFetched = numBlocksFetched;
            return this;
        }

        public bool IsReceivedMoney()
        {
            return _receivedMoney;
        }

        public MoneroSyncResult SetReceivedMoney(bool receivedMoney)
        {
            _receivedMoney = receivedMoney;
            return this;
        }

        public MoneroSyncResult Clone()
        {
            return new MoneroSyncResult(this);
        }
    }
}
