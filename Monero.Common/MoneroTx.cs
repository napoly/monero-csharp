
namespace Monero.Common
{
    public class MoneroTx
    {
        public static readonly string DEFAULT_PAYMENT_ID = "0000000000000000";

        private MoneroBlock? block;
        private string? hash;
        private uint? version;
        private bool? isMinerTx;
        private string? paymentId;
        private ulong? fee;
        private uint? ringSize;
        private bool? relay;
        private bool? isRelayed;
        private bool? isConfirmed;
        private bool? inTxPool;
        private ulong? numConfirmations;
        private ulong? unlockTime;
        private ulong? lastRelayedTimestamp;
        private ulong? receivedTimestamp;
        private bool? isDoubleSpendSeen;
        private string? key;
        private string? fullHex;
        private string? prunedHex;
        private string? prunableHex;
        private string? prunableHash;
        private ulong? size;
        private ulong? weight;
        private List<MoneroOutput> inputs = [];
        private List<MoneroOutput> outputs = [];
        private List<ulong> outputIndices = [];
        private string? metadata;
        private byte[]? extra; // TODO: switch to string for consistency with MoneroTxWallet?
        private object? rctSignatures; // TODO: implement
        private object? rctSigPrunable;  // TODO: implement
        private bool? isKeptByBlock;
        private bool? isFailed;
        private ulong? lastFailedHeight;
        private string? lastFailedHash;
        private ulong? maxUsedBlockHeight;
        private string? maxUsedBlockHash;
        private List<string> signatures = [];

        public MoneroTx()
        {
            // nothing to build
        }

        public MoneroTx(MoneroTx tx)
        {
            this.hash = tx.hash;
            this.version = tx.version;
            this.isMinerTx = tx.isMinerTx;
            this.paymentId = tx.paymentId;
            this.fee = tx.fee;
            this.ringSize = tx.ringSize;
            this.relay = tx.relay;
            this.isRelayed = tx.isRelayed;
            this.isConfirmed = tx.isConfirmed;
            this.inTxPool = tx.inTxPool;
            this.numConfirmations = tx.numConfirmations;
            this.unlockTime = tx.unlockTime;
            this.lastRelayedTimestamp = tx.lastRelayedTimestamp;
            this.receivedTimestamp = tx.receivedTimestamp;
            this.isDoubleSpendSeen = tx.isDoubleSpendSeen;
            this.key = tx.key;
            this.fullHex = tx.fullHex;
            this.prunedHex = tx.prunedHex;
            this.prunableHex = tx.prunableHex;
            this.prunableHash = tx.prunableHash;
            this.size = tx.size;
            this.weight = tx.weight;
            if (tx.inputs != null)
            {
                this.inputs = new List<MoneroOutput>();
                foreach (MoneroOutput input in tx.inputs) inputs.Add(input.Clone().SetTx(this));
            }
            if (tx.outputs != null)
            {
                this.outputs = new List<MoneroOutput>();
                foreach (MoneroOutput output in tx.outputs) outputs.Add(output.Clone().SetTx(this));
            }
            if (tx.outputIndices != null) this.outputIndices = new List<ulong>(tx.outputIndices);
            this.metadata = tx.metadata;
            if (tx.extra != null) this.extra = tx.extra;
            this.rctSignatures = tx.rctSignatures;
            this.rctSigPrunable = tx.rctSigPrunable;
            this.isKeptByBlock = tx.isKeptByBlock;
            this.isFailed = tx.isFailed;
            this.lastFailedHeight = tx.lastFailedHeight;
            this.lastFailedHash = tx.lastFailedHash;
            this.maxUsedBlockHeight = tx.maxUsedBlockHeight;
            this.maxUsedBlockHash = tx.maxUsedBlockHash;
            if (tx.signatures != null) this.signatures = new List<string>(tx.signatures);
        }

        public virtual MoneroTx Clone()
        {
            return new MoneroTx(this);
        }

        public MoneroBlock? GetBlock()
        {
            return block;
        }

        public MoneroTx SetBlock(MoneroBlock block)
        {
            this.block = block;
            return this;
        }

        public ulong? GetHeight()
        {
            return GetBlock() == null ? null : GetBlock().GetHeight();
        }

        public string? GetHash()
        {
            return hash;
        }

        public virtual MoneroTx SetHash(string hash)
        {
            this.hash = hash;
            return this;
        }

        public uint? GetVersion()
        {
            return version;
        }

        public MoneroTx SetVersion(uint version)
        {
            this.version = version;
            return this;
        }

        public bool? IsMinerTx()
        {
            return isMinerTx;
        }

        public MoneroTx SetIsMinerTx(bool IsMinerTx)
        {
            this.isMinerTx = IsMinerTx;
            return this;
        }

        public string? GetPaymentId()
        {
            return paymentId;
        }

        public MoneroTx SetPaymentId(string paymentId)
        {
            this.paymentId = paymentId;
            return this;
        }

        public ulong? GetFee()
        {
            return fee;
        }

        public MoneroTx SetFee(ulong fee)
        {
            this.fee = fee;
            return this;
        }

        public uint? GetRingSize()
        {
            return ringSize;
        }

        public MoneroTx SetRingSize(uint ringSize)
        {
            this.ringSize = ringSize;
            return this;
        }

        public bool? GetRelay()
        {
            return relay;
        }

        public MoneroTx SetRelay(bool relay)
        {
            this.relay = relay;
            return this;
        }

        public bool? IsRelayed()
        {
            return isRelayed;
        }

        public MoneroTx SetIsRelayed(bool IsRelayed)
        {
            isRelayed = IsRelayed;
            return this;
        }

        public bool? IsConfirmed()
        {
            return isConfirmed;
        }

        public MoneroTx SetIsConfirmed(bool IsConfirmed)
        {
            this.isConfirmed = IsConfirmed;
            return this;
        }

        public bool? InTxPool()
        {
            return inTxPool;
        }

        public MoneroTx SetInTxPool(bool inTxPool)
        {
            this.inTxPool = inTxPool;
            return this;
        }

        public ulong? GetNumConfirmations()
        {
            return numConfirmations;
        }

        public MoneroTx SetNumConfirmations(ulong numConfirmations)
        {
            this.numConfirmations = numConfirmations;
            return this;
        }

        public ulong? GetUnlockTime()
        {
            return unlockTime;
        }

        public virtual MoneroTx SetUnlockTime(ulong? unlockTime)
        {
            this.unlockTime = unlockTime;
            return this;
        }

        public ulong? GetLastRelayedTimestamp()
        {
            return lastRelayedTimestamp;
        }

        public MoneroTx SetLastRelayedTimestamp(ulong lastRelayedTimestamp)
        {
            this.lastRelayedTimestamp = lastRelayedTimestamp;
            return this;
        }

        public ulong? GetReceivedTimestamp()
        {
            return receivedTimestamp;
        }

        public MoneroTx SetReceivedTimestamp(ulong receivedTimestamp)
        {
            this.receivedTimestamp = receivedTimestamp;
            return this;
        }

        public bool? IsDoubleSpendSeen()
        {
            return isDoubleSpendSeen;
        }

        public MoneroTx SetIsDoubleSpendSeen(bool IsDoubleSpend)
        {
            this.isDoubleSpendSeen = IsDoubleSpend;
            return this;
        }

        public string? GetKey()
        {
            return key;
        }

        public MoneroTx SetKey(string key)
        {
            this.key = key;
            return this;
        }

        public string? GetFullHex()
        {
            return fullHex;
        }

        public MoneroTx SetFullHex(string fullHex)
        {
            this.fullHex = fullHex;
            return this;
        }

        public string? GetPrunedHex()
        {
            return prunedHex;
        }

        public MoneroTx SetPrunedHex(string prunedHex)
        {
            this.prunedHex = prunedHex;
            return this;
        }

        public string? GetPrunableHex()
        {
            return prunableHex;
        }

        public MoneroTx SetPrunableHex(string prunableHex)
        {
            this.prunableHex = prunableHex;
            return this;
        }

        public string? GetPrunableHash()
        {
            return prunableHash;
        }

        public MoneroTx SetPrunableHash(string prunableHash)
        {
            this.prunableHash = prunableHash;
            return this;
        }

        public ulong? GetSize()
        {
            return size;
        }

        public MoneroTx SetSize(ulong size)
        {
            this.size = size;
            return this;
        }

        public ulong? GetWeight()
        {
            return weight;
        }

        public MoneroTx SetWeight(ulong weight)
        {
            this.weight = weight;
            return this;
        }

        public List<MoneroOutput> GetInputs()
        {
            return inputs;
        }

        public virtual MoneroTx SetInputs(List<MoneroOutput> inputs)
        {
            this.inputs = inputs;
            return this;
        }

        public List<MoneroOutput> GetOutputs()
        {
            return outputs;
        }

        public virtual MoneroTx SetOutputs(List<MoneroOutput> outputs)
        {
            this.outputs = outputs;
            return this;
        }

        public List<ulong> GetOutputIndices()
        {
            return outputIndices;
        }

        public MoneroTx SetOutputIndices(List<ulong> outputIndices)
        {
            this.outputIndices = outputIndices;
            return this;
        }

        public string? GetMetadata()
        {
            return metadata;
        }

        public MoneroTx SetMetadata(string metadata)
        {
            this.metadata = metadata;
            return this;
        }

        public byte[]? GetExtra()
        {
            return extra;
        }

        public MoneroTx SetExtra(byte[] extra)
        {
            this.extra = extra;
            return this;
        }

        public object? GetRctSignatures()
        {
            return rctSignatures;
        }

        public MoneroTx SetRctSignatures(object rctSignatures)
        {
            this.rctSignatures = rctSignatures;
            return this;
        }

        public object? GetRctSigPrunable()
        {
            return rctSigPrunable;
        }

        public MoneroTx SetRctSigPrunable(object rctSigPrunable)
        {
            this.rctSigPrunable = rctSigPrunable;
            return this;
        }

        public bool? IsKeptByBlock()
        {
            return isKeptByBlock;
        }

        public MoneroTx SetIsKeptByBlock(bool IsKeptByBlock)
        {
            isKeptByBlock = IsKeptByBlock;
            return this;
        }

        public bool? IsFailed()
        {
            return isFailed;
        }

        public MoneroTx SetIsFailed(bool IsFailed)
        {
            this.isFailed = IsFailed;
            return this;
        }

        public ulong? GetLastFailedHeight()
        {
            return lastFailedHeight;
        }

        public MoneroTx SetLastFailedHeight(ulong lastFailedHeight)
        {
            this.lastFailedHeight = lastFailedHeight;
            return this;
        }

        public string? GetLastFailedHash()
        {
            return lastFailedHash;
        }

        public MoneroTx SetLastFailedHash(string lastFailedHash)
        {
            this.lastFailedHash = lastFailedHash;
            return this;
        }

        public ulong? GetMaxUsedBlockHeight()
        {
            return maxUsedBlockHeight;
        }

        public MoneroTx SetMaxUsedBlockHeight(ulong maxUsedBlockHeight)
        {
            this.maxUsedBlockHeight = maxUsedBlockHeight;
            return this;
        }

        public string? GetMaxUsedBlockHash()
        {
            return maxUsedBlockHash;
        }

        public MoneroTx SetMaxUsedBlockHash(string maxUsedBlockHash)
        {
            this.maxUsedBlockHash = maxUsedBlockHash;
            return this;
        }

        public List<string> GetSignatures()
        {
            return signatures;
        }

        public MoneroTx SetSignatures(List<string> signatures)
        {
            this.signatures = signatures;
            return this;
        }
    }
}
