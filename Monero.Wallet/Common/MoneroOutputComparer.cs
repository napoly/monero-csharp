using Monero.Common;

namespace Monero.Wallet.Common
{
    internal class MoneroOutputComparer : Comparer<MoneroOutput>
    {
        private static readonly MoneroTxHeightComparer TX_HEIGHT_COMPARATOR = new MoneroTxHeightComparer();

        public override int Compare(MoneroOutput? o1, MoneroOutput? o2)
        {
            MoneroOutputWallet ow1 = (MoneroOutputWallet)o1;
            MoneroOutputWallet ow2 = (MoneroOutputWallet)o2;

            // compare by height
            int heightComparison = TX_HEIGHT_COMPARATOR.Compare(ow1.GetTx(), ow2.GetTx());
            if (heightComparison != 0) return heightComparison;

            // compare by account index, subaddress index, output index, then key image hex
            int compare = ow1.GetAccountIndex().CompareTo(ow2.GetAccountIndex());
            if (compare != 0) return compare;
            compare = ow1.GetSubaddressIndex().CompareTo(ow2.GetSubaddressIndex());
            if (compare != 0) return compare;
            compare = ow1.GetIndex().CompareTo(ow2.GetIndex());
            if (compare != 0) return compare;
            return ow1.GetKeyImage().GetHex().CompareTo(ow2.GetKeyImage().GetHex());
        }
    }
}
