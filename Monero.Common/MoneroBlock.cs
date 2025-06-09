
namespace Monero.Common
{
    public class MoneroBlock : MoneroBlockHeader
    {
        private string hex;
        private MoneroTx minerTx;
        private List<MoneroTx> txs;
        private List<string> txHashes;

        public MoneroBlock(MoneroBlock block) : base(block)
        {
            this.hex = block.GetHex();
            if (block.minerTx != null) this.minerTx = block.minerTx.Clone().SetBlock(this);
            if (block.txs != null)
            {
                this.txs = new List<MoneroTx>();
                foreach (MoneroTx tx in block.txs) txs.Add(tx.Clone().setBlock(this));
            }
            if (block.GetTxHashes() != null) this.txHashes = new List<String>(block.GetTxHashes());
        }

        public string GetHex()
        {
            return hex;
        }

        public MoneroBlock SetHex(string hex)
        {
            this.hex = hex;
            return this;
        }

        public MoneroTx GetMinerTx()
        {
            return minerTx;
        }

        public MoneroBlock SetMinerTx(MoneroTx minerTx)
        {
            this.minerTx = minerTx;
            return this;
        }

        public List<MoneroTx> GetTxs()
        {
            return txs;
        }

        public MoneroBlock SetTxs(List<MoneroTx> txs)
        {
            this.txs = txs;
            return this;
        }

        public List<string> GetTxHashes()
        {
            return txHashes;
        }

        public MoneroBlock SetTxHashes(List<string> txHashes)
        {
            this.txHashes = txHashes;
            return this;
        }

        public MoneroBlock Clone()
        {
            return new MoneroBlock(this);
        }
    }
}
