
using Monero.Wallet.Common;

namespace Monero.Wallet
{
    public abstract class MoneroWalletListener
    {
        public abstract void OnSyncProgress(long height, long startHeight, long endHeight, double percentDone, string message);
        public abstract void OnNewBlock(long height);
        public abstract void OnBalancesChanged(long newBalance, long newUnlockedBalance);
        public abstract void OnOutputReceived(MoneroOutputWallet output);
        public abstract void OnOutputSpent(MoneroOutputWallet output);
    }
}
