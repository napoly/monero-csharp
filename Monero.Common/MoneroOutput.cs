
namespace Monero.Common
{
    public class MoneroOutput
    {
        public MoneroTx Tx;

        private MoneroTx tx;
        private MoneroKeyImage keyImage;
        private ulong amount;
        private ulong index;
        private List<ulong> ringOutputIndices;
        private string stealthPublicKey;

        public MoneroOutput()
        {
            // nothing to build
        }

        public MoneroOutput(MoneroOutput output)
        {
            if (output.keyImage != null) this.keyImage = output.keyImage.Clone();
            amount = output.amount;
            index = output.index;
            if (output.ringOutputIndices != null) ringOutputIndices = new List<ulong>(output.ringOutputIndices);
            stealthPublicKey = output.stealthPublicKey;
        }

        public MoneroOutput Clone()
        {
            return new MoneroOutput(this);
        }

        public MoneroTx GetTx()
        {
            return tx;
        }

        public MoneroOutput SetTx(MoneroTx tx)
        {
            this.tx = tx;
            return this;
        }

        public MoneroKeyImage GetKeyImage()
        {
            return keyImage;
        }

        public MoneroOutput SetKeyImage(MoneroKeyImage keyImage)
        {
            this.keyImage = keyImage;
            return this;
        }

        public ulong GetAmount()
        {
            return amount;
        }

        public MoneroOutput SetAmount(ulong amount)
        {
            this.amount = amount;
            return this;
        }

        public ulong GetIndex()
        {
            return index;
        }

        public MoneroOutput SetIndex(ulong index)
        {
            this.index = index;
            return this;
        }

        public List<ulong> GetRingOutputIndices()
        {
            return ringOutputIndices;
        }

        public MoneroOutput SetRingOutputIndices(List<ulong> ringOutputIndices)
        {
            this.ringOutputIndices = ringOutputIndices;
            return this;
        }

        public string GetStealthPublicKey()
        {
            return stealthPublicKey;
        }

        public MoneroOutput SetStealthPublicKey(string stealthPublicKey)
        {
            this.stealthPublicKey = stealthPublicKey;
            return this;
        }
    }
}
