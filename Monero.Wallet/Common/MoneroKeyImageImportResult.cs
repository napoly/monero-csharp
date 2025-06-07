
namespace Monero.Wallet.Common
{
    public class MoneroKeyImageImportResult
    {
        private long _height;
        private long _spentAmount;
        private long _unspentAmount;

        public MoneroKeyImageImportResult(long height, long spentAmount, long unspentAmount)
        {
            _height = height;
            _spentAmount = spentAmount;
            _unspentAmount = unspentAmount;
        }

        public MoneroKeyImageImportResult(MoneroKeyImageImportResult importResult)
        {
            _height = importResult._height;
            _spentAmount = importResult._spentAmount;
            _unspentAmount = importResult._unspentAmount;
        }

        public long GetHeight()
        {
            return _height;
        }

        public MoneroKeyImageImportResult SetHeight(long height)
        {
            _height = height;
            return this;
        }

        public long GetSpentAmount()
        {
            return _spentAmount;
        }

        public MoneroKeyImageImportResult SetSpentAmount(long spentAmount)
        {
            _spentAmount = spentAmount;
            return this;
        }

        public long GetUnspentAmount()
        {
            return _unspentAmount;
        }

        public MoneroKeyImageImportResult SetUnspentAmount(long unspentAmount)
        {
            _unspentAmount = unspentAmount;
            return this;
        }

        public MoneroKeyImageImportResult Clone()
        {
            return new MoneroKeyImageImportResult(this);
        }
    }
}
