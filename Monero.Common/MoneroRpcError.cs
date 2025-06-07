
namespace Monero.Common
{
    public class MoneroRpcError : MoneroError
    {
        private readonly string _rpcMethod;


        public MoneroRpcError(string message, int code, string rpcMethod) : base(message, code)
        {
            _rpcMethod = rpcMethod;
        }

        public string GetRpcMethod()
        {
            return _rpcMethod;
        }
    }
}
