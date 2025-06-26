
namespace Monero.Common
{
    public class MoneroRpcConnection : MoneroConnection
    {
        private string? _username;
        private string? _password;
        private string? _zmqUri;
        private bool? _isAuthenticated;
        
        private bool _printStackTrace = false;
        
        public MoneroRpcConnection(string? uri = null, string? username = null, string? password = null, string? zmqUri = null, int priority = 0) : base(uri, null, priority)
        {
            _username = username;
            _password = password;
            _zmqUri = zmqUri;
        }

        public bool Equals(MoneroRpcConnection? other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) &&
                   _username == other._username &&
                   _password == other._password &&
                   _zmqUri == other._zmqUri &&
                   _isAuthenticated == other._isAuthenticated;
        }

        public MoneroRpcConnection(MoneroRpcConnection other) : base(other)
        {
            _username = other._username;
            _password = other._password;
            _zmqUri = other._zmqUri;
            _isAuthenticated = other._isAuthenticated;
            _printStackTrace = other._printStackTrace;
        }

        public override MoneroRpcConnection Clone()
        {
            return new MoneroRpcConnection(this);
        }

        public override bool? IsConnected()
        {
            if (_isAuthenticated != null) return _isOnline == true && _isAuthenticated == true;
            return _isOnline;
        }

        public override bool? IsAuthenticated()
        {
            return _isAuthenticated;
        }

        public override bool CheckConnection(ulong timeoutMs)
        {
            lock (this)
            {
                bool? isOnlineBefore = _isOnline;
                bool? isAuthenticatedBefore = _isAuthenticated;
                ulong startTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                try
                {
                    var request = new MoneroJsonRpcRequest("get_version");
                    SendJsonRequest(request, timeoutMs);

                    _isOnline = true;
                    _isAuthenticated = true;
                }
                catch (Exception e)
                {
                    _isOnline = false;
                    _isAuthenticated = null;
                    _responseTime = null;

                    if (e as MoneroRpcError != null)
                    {
                        if (((MoneroRpcError)e).GetCode() == 401)
                        {
                            _isOnline = true;
                            _isAuthenticated = false;
                        }
                        else if (((MoneroRpcError)e).GetCode() == 404)
                        { // fallback to latency check
                            _isOnline = true;
                            _isAuthenticated = true;
                        }
                    }
                }
                var now = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                if (_isOnline == true) _responseTime = now - (ulong)startTime;
                return isOnlineBefore != _isOnline || isAuthenticatedBefore != _isAuthenticated;
            }
        }

        public override MoneroRpcConnection SetAttribute(string key, string value)
        {
            _attributes[key] = value;
            return this;
        }

        public override MoneroRpcConnection SetUri(string? uri)
        {
            _uri = uri;
            return this;
        }

        public override MoneroRpcConnection SetProxyUri(string? uri)
        {
            _proxyUri = uri;
            return this;
        }

        public override MoneroRpcConnection SetPriority(int priority)
        {
            _priority = priority;
            return this;
        }

        public string? GetUsername()
        {
            return _username;
        }

        public string? GetPassword()
        {
            return _password;
        }

        public MoneroRpcConnection SetCredentials(string? username, string? password)
        {
            _username = username;
            _password = password;
            return this;
        }

        public string? GetZmqUri()
        {
            return _zmqUri;
        }

        public MoneroRpcConnection SetZmqUri(string? zmqUri)
        {
            _zmqUri = zmqUri;
            return this;
        }

        public object SendJsonRequest(MoneroJsonRpcRequest request, ulong timeoutMs = 2000)
        {
            throw new NotImplementedException("SendJsonRequest(): not implemented");
        }

        public MoneroJsonRpcResponse SendJsonRequest(string method, List<string> parameters, ulong timeoutMs = 2000)
        {
            throw new NotImplementedException("SendJsonRequest(): not implemented");
        }

        public MoneroJsonRpcResponse SendJsonRequest(string method, Dictionary<string, object>? parameters = null, ulong timeoutMs = 2000)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> SendPathRequest(string method, Dictionary<string, object>? parameters = null, ulong timeoutMs = 2000)
        {
            throw new NotImplementedException("SendPathRequest(): not implemented");
        }

        public byte[] SendBinaryRequest(string method, Dictionary<string, object>? parameters = null, ulong timeoutMs = 2000)
        {
            throw new NotImplementedException("SendBinaryRequest(): not implemented");
        }
    }
}
