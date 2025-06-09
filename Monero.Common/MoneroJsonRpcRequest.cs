using Newtonsoft.Json;

namespace Monero.Common
{

    public class MoneroJsonRpcRequest : MoneroHttpRequest
    {
        [JsonProperty("jsonrpc")]
        public readonly static string Version = "2.0";

        [JsonProperty("method")]
        public readonly string Method;

        public MoneroJsonRpcRequest(string method)
        {
            Method = method;
        }
    }

    public class MoneroJsonRpcRequest<T> : MoneroJsonRpcRequest
    {
        [JsonProperty("params")]
        public T Params;

        public MoneroJsonRpcRequest(string method, T parameters) : base(method)
        {
            Params = parameters;
        }
    }
}
