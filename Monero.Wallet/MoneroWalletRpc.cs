
using Monero.Common;

namespace Monero.Wallet
{
    public class MoneroWalletRpc : MoneroWalletDefault
    {
        private MoneroRpcConnection rpc;

        public MoneroWalletRpc(MoneroRpcConnection connection) { rpc = connection; }

        public MoneroRpcConnection GetRpcConnection() { return rpc; }
    }
}
