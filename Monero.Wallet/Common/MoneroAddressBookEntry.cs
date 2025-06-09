
namespace Monero.Wallet.Common
{
    public class MoneroAddressBookEntry
    {
        private int index;
        private string address;
        private string paymentId;
        private string description;

        public MoneroAddressBookEntry(int index, string address, string description, string paymentId)
        {
            this.index = index;
            this.address = address;
            this.paymentId = paymentId;
            this.description = description;
        }


        public int GetIndex()
        {
            return index;
        }

        public MoneroAddressBookEntry SetIndex(int index)
        {
            this.index = index;
            return this;
        }

        public string GetAddress()
        {
            return address;
        }

        public MoneroAddressBookEntry SetAddress(string address)
        {
            this.address = address;
            return this;
        }

        public string GetPaymentId()
        {
            return paymentId;
        }

        public MoneroAddressBookEntry SetPaymentId(string paymentId)
        {
            this.paymentId = paymentId;
            return this;
        }

        public string GetDescription()
        {
            return description;
        }

        public MoneroAddressBookEntry SetDescription(string description)
        {
            this.description = description;
            return this;
        }

    }
}
