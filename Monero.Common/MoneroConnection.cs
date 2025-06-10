
namespace Monero.Common
{
    public abstract class MoneroConnection
    {
        public static readonly long DEFAULT_TIMEOUT = 2000;
        protected string? _uri;
        protected string? _proxyUri;
        protected int _priority;
        protected long _timeoutMs;
        protected long? _responseTime;
        protected Dictionary<string, string?> _attributes = [];
        protected bool? _isOnline;

        public MoneroConnection(string? uri = null, string? proxyUri = null, int priority = 0, long timeoutMs = 2000)
        {
            _uri = uri;
            _proxyUri = proxyUri;
            _priority = priority;
            _timeoutMs = timeoutMs;
        }

        public MoneroConnection(MoneroConnection other)
        {
            _uri = other._uri;
            _proxyUri = other._proxyUri;
            _priority = other._priority;
            _timeoutMs = other._timeoutMs;
            _isOnline = other._isOnline;
            _responseTime = other._responseTime;
        }

        public abstract bool? IsAuthenticated();

        public virtual bool? IsOnline()
        {
            return _isOnline;
        }

        public string? GetAttribute(string key)
        {
            return _attributes.GetValueOrDefault(key, null);
        }

        public virtual MoneroConnection SetAttribute(string key, string value)
        {
            _attributes[key] = value;
            return this;
        }

        public virtual string? GetUri()
        {
            return _uri;
        }

        public virtual MoneroConnection SetUri(string? uri)
        {
            _uri = uri;
            return this;
        }

        public string? GetProxyUri()
        {
            return _proxyUri;
        }

        public virtual MoneroConnection SetProxyUri(string? uri)
        {
            _proxyUri = uri;
            return this;
        }
        
        public int GetPriority()
        {
            return _priority;
        }

        public virtual MoneroConnection SetPriority(int priority)
        {
            _priority = priority;
            return this;
        }

        public long GetTimeout() { return _timeoutMs; }

        public MoneroConnection SetTimeout(long timeoutMs)
        {
            _timeoutMs = timeoutMs;
            return this;
        }

        public long? GetResponseTime()
        {
            return _responseTime;
        }

        public abstract bool? IsConnected();

        public abstract bool CheckConnection(long timeoutMs);

        public abstract MoneroConnection Clone();

    }
}
