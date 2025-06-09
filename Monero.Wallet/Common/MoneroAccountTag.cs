
namespace Monero.Wallet.Common
{
    public class MoneroAccountTag
    {
        private string _tag;
        private string _label;
        private List<uint> _accountIndices;

        public MoneroAccountTag(string tag, string label, List<uint> accountIndices)
        {
            _tag = tag;
            _label = label;
            _accountIndices = accountIndices ?? new List<uint>();
        }

        public MoneroAccountTag(string tag, string label)
        {
            _tag = tag;
            _label = label;
            _accountIndices = [];
        }

        public MoneroAccountTag(string tag)
        {
            _tag = tag;
            _label = string.Empty;
            _accountIndices = [];
        }

        public string GetTag()
        {
            return _tag;
        }

        public MoneroAccountTag SetTag(string tag)
        {
            _tag = tag;
            return this;
        }

        public string GetLabel()
        {
            return _label;
        }

        public MoneroAccountTag SetLabel(string label)
        {
            _label = label;
            return this;
        }

        public List<uint> GetAccountIndices()
        {
            return _accountIndices;
        }

        public MoneroAccountTag SetAccountIndices(List<uint>? accountIndices)
        {
            _accountIndices = accountIndices ?? [];
            return this;
        }
    }
}
