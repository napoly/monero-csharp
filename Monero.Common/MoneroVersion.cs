
namespace Monero.Common
{
    public class MoneroVersion
    {
        private int _number;
        private bool _isRelease;

        public MoneroVersion(int number, bool isRelease)
        {
            _number = number;
            _isRelease = isRelease;
        }

        public int GetNumber() { return _number; }
        public bool IsRelease() { return _isRelease; }

        public MoneroVersion SetNumber(int number) { 
            _number = number;
            return this;
        }

        public MoneroVersion SetIsRelease(bool isRelease)
        {
            _isRelease = isRelease;
            return this;
        }

    }
}
