using Newtonsoft.Json;

namespace Monero.Common
{

    public class MoneroJsonRpcRequest : MoneroHttpRequest
    {
        [JsonProperty("jsonrpc", Order = 0)]
        public readonly static string Version = "2.0";

        [JsonProperty("method", Order = 1)]
        public readonly string Method;

        [JsonProperty("params", Order = 2, NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object>? Params;

        public MoneroJsonRpcRequest(string method, Dictionary<string, object>? parameters = null)
        {
            Method = method;
            Params = parameters;
        }

    }

    public class MoneroJsonRpcResponse
    {
        [JsonProperty("result")]
        public Dictionary<string, object>? Result;
    }
}
