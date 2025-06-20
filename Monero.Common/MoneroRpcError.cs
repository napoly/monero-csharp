
using MoneroJsonRpcParams = System.Collections.Generic.Dictionary<string, object>;

namespace Monero.Common
{
    public class MoneroRpcError : MoneroError
    {
        private readonly string _rpcMethod;
        private readonly MoneroJsonRpcParams? _params;

        public MoneroRpcError(string message, int? code, string rpcMethod, MoneroJsonRpcParams? parameters = null) : base(message, code)
        {
            _rpcMethod = rpcMethod;
            _params = parameters;
        }

        public string GetRpcMethod()
        {
            return _rpcMethod;
        }

        public MoneroJsonRpcParams? GetRpcParams() {
            return _params;
        }
    }
}
