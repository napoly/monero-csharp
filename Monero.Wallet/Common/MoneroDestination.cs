
namespace Monero.Wallet.Common
{
    public class MoneroDestination
    {
        private string _address;
        private long _amount;

        public MoneroDestination(string address, long amount)
        {
            _address = address;
            _amount = amount;
        }

        public MoneroDestination(MoneroDestination destination)
        {
            _address = destination._address;
            _amount = destination._amount;
        }

        public string GetAddress()
        {
            return _address;
        }

        public MoneroDestination SetAddress(string address)
        {
            _address = address;
            return this;
        }

        public long GetAmount()
        {
            return _amount;
        }

        public MoneroDestination SetAmount(long amount)
        {
            _amount = amount;
            return this;
        }
    }
}
