
namespace Monero.Wallet.Common
{
    public abstract class MoneroTransfer
    {
        protected MoneroTxWallet _tx;
        protected ulong _amount;
        protected uint _accountIndex;

        public MoneroTransfer() { }

        public MoneroTransfer(MoneroTransfer transfer)
        {
            _tx = transfer._tx;
            _amount = transfer._amount;
            _accountIndex = transfer._accountIndex;
        }

        public abstract MoneroTransfer Clone();

        public MoneroTxWallet GetTx() {
            return _tx;
        }

        public virtual MoneroTransfer SetTx(MoneroTxWallet tx) {
            _tx = tx;
            return this;
        }

        public abstract bool? IsIncoming();

        public virtual bool? IsOutgoing() {
            return !IsIncoming();
        }

        public ulong GetAmount() {
            return _amount;
        }

        public MoneroTransfer SetAmount(ulong amount) {
            _amount = amount;
            return this;
        }

        public uint GetAccountIndex() {
            return _accountIndex;
        }

        public MoneroTransfer SetAccountIndex(uint accountIndex) {
            _accountIndex = accountIndex;
            return this;
        }

    }
}
