using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace Monero.Common
{
    public class MoneroRpcConnection
    {
        private string _uri;
        private string? _username;
        private string? _password;
        private string? _zmqUri;
        private int _priority = 0;
        private long _timeoutMs;
        private bool _isOnline = false;
        private bool? _isAuthenticated = false;
        private long? _responseTime;
        private string? _proxyUri;
        private bool _printStackTrace = false;
        private Dictionary<string, string> _attributes = [];
        
        public MoneroRpcConnection(string uri, string? username, string? password, string? zmqUri)
        {
            _uri = uri;
            _username = username;
            _password = password;
            _zmqUri = zmqUri;
        }

        public MoneroRpcConnection(MoneroRpcConnection other)
        {
            _uri = other._uri;
            _username = other._username;
            _password = other._password;
            _zmqUri = other._zmqUri;
            _priority = other._priority;
            _timeoutMs = other._timeoutMs;
            _isOnline = other._isOnline;
            _isAuthenticated = other._isAuthenticated;
            _responseTime = other._responseTime;
            _proxyUri = other._proxyUri;
            _printStackTrace = other._printStackTrace;
            _attributes = new Dictionary<string, string>(other._attributes);
        }

        public string GetUri()
        {
            return _uri;
        }

        public MoneroRpcConnection SetUri(string uri)
        {
            _uri = uri;
            return this;
        }

        public string? GetUsername()
        {
            return _username;
        }

        public MoneroRpcConnection SetUsername(string? username)
        {
            _username = username;
            return this;
        }

        public string? GetPassword()
        {
            return _password;
        }

        public MoneroRpcConnection SetPassword(string? password)
        {
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

        public int GetPriority()
        {
            return _priority;
        }

        public MoneroRpcConnection SetPriority(int priority)
        {
            _priority = priority;
            return this;
        }

        public long GetTimeout() { return _timeoutMs; }

        public MoneroRpcConnection SetTimeout(long timeoutMs)
        {
            _timeoutMs = timeoutMs;
            return this;
        }

        public bool? IsOnline()
        {
            return _isOnline;
        }

        public bool IsConnected()
        {
            if (_isAuthenticated != null) return _isOnline && (bool)_isAuthenticated;
            return _isOnline;
        }
    
        public long? GetResponseTime()
        {
            return _responseTime;
        }

        public bool CheckConnection(long timeoutMs)
        {
            lock(this) {
                bool isOnlineBefore = _isOnline;
                bool? isAuthenticatedBefore = _isAuthenticated;
                long startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                try
                {
                    sendJsonRequest("get_version", null, timeoutMs);

                    _isOnline = true;
                    _isAuthenticated = true;
                }
                catch (Exception e)
                {
                    _isOnline = false;
                    _isAuthenticated = null;
                    _responseTime = null;
                    
                    if (e as MoneroRpcError != null) {
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
                if (_isOnline) _responseTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - startTime;
                return isOnlineBefore != _isOnline || isAuthenticatedBefore != _isAuthenticated;
            }
        }
    }
}
