using Monero.Wallet;

namespace Monero.Test
{
    internal abstract class TestUtils
    {
        public static readonly int SYNC_PERIOD_IN_MS = 10000;
        public static readonly int AUTO_CONNECT_TIMEOUT_MS = 2000;
        public static readonly string WALLET_RPC_USERNAME = "";
        public static readonly string WALLET_RPC_PASSWORD = "";

        public static MoneroWalletRpc StartWalletRpcProcess()
        {
            throw new NotImplementedException();
        }

        public static void StopWalletRpcProcess(MoneroWalletRpc wallet)
        {
            throw new NotImplementedException();
        }

    }
}
