
namespace Monero.Wallet.Common
{
    public class MoneroMultisigSignResult
    {
        private string _signedMultisigHex;
        private List<string> _txHashes;

        public MoneroMultisigSignResult(string signedMultisigHex, List<string> txHashes)
        {
            _signedMultisigHex = signedMultisigHex;
            _txHashes = txHashes;
        }

        public MoneroMultisigSignResult(MoneroMultisigSignResult signResult)
        {
            _signedMultisigHex = signResult._signedMultisigHex;
            _txHashes = new List<string>(signResult._txHashes);
        }

        public string GetSignedMultisigHex()
        {
            return _signedMultisigHex;
        }

        public MoneroMultisigSignResult SetSignedMultisigHex(string signedMultisigHex)
        {
            _signedMultisigHex = signedMultisigHex;
            return this;
        }

        public List<string> GetTxHashes()
        {
            return _txHashes;
        }

        public MoneroMultisigSignResult SetTxHashes(List<string> txHashes)
        {
            _txHashes = new List<string>(txHashes);
            return this;
        }

        public MoneroMultisigSignResult Clone()
        {
            return new MoneroMultisigSignResult(this);
        }
    }
}
