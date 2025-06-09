
using System.Numerics;
using System.Runtime.InteropServices;

namespace Monero.Common
{
    public class MoneroBlockHeader
    {
        private string hash;
        private ulong height;
        private ulong timestamp;
        private ulong size;
        private ulong weight;
        private ulong longTermWeight;
        private ulong depth;
        private ulong difficulty;
        private ulong cumulativeDifficulty;
        private uint majorVersion;
        private uint minorVersion;
        private ulong nonce;
        private string minerTxHash;
        private uint numTxs;
        private bool orphanStatus;
        private string prevHash;
        private ulong reward;
        private string powHash;

        public MoneroBlockHeader(MoneroBlockHeader header)
        {
            hash = header.hash;
            height = header.height;
            timestamp = header.timestamp;
            size = header.size;
            weight = header.weight;
            longTermWeight = header.longTermWeight;
            depth = header.depth;
            difficulty = header.difficulty;
            cumulativeDifficulty = header.cumulativeDifficulty;
            majorVersion = header.majorVersion;
            minorVersion = header.minorVersion;
            nonce = header.nonce;
            numTxs = header.numTxs;
            orphanStatus = header.orphanStatus;
            prevHash = header.prevHash;
            reward = header.reward;
            powHash = header.powHash;
        }

        public string GetHash()
        {
            return hash;
        }

        public MoneroBlockHeader SetHash(string hash)
        {
            this.hash = hash;
            return this;
        }

        public ulong GetHeight()
        {
            return height;
        }

        public MoneroBlockHeader SetHeight(ulong height)
        {
            this.height = height;
            return this;
        }

        public ulong GetTimestamp()
        {
            return timestamp;
        }

        public MoneroBlockHeader SetTimestamp(ulong timestamp)
        {
            this.timestamp = timestamp;
            return this;
        }

        public ulong GetSize()
        {
            return size;
        }

        public MoneroBlockHeader SetSize(ulong size)
        {
            this.size = size;
            return this;
        }

        public ulong GetWeight()
        {
            return weight;
        }

        public MoneroBlockHeader SetWeight(ulong weight)
        {
            this.weight = weight;
            return this;
        }

        public ulong GetLongTermWeight()
        {
            return longTermWeight;
        }

        public MoneroBlockHeader SetLongTermWeight(ulong longTermWeight)
        {
            this.longTermWeight = longTermWeight;
            return this;
        }

        public ulong GetDepth()
        {
            return depth;
        }

        public MoneroBlockHeader SetDepth(ulong depth)
        {
            this.depth = depth;
            return this;
        }

        public ulong GetDifficulty()
        {
            return difficulty;
        }

        public MoneroBlockHeader SetDifficulty(ulong difficulty)
        {
            this.difficulty = difficulty;
            return this;
        }

        public ulong GetCumulativeDifficulty()
        {
            return cumulativeDifficulty;
        }

        public MoneroBlockHeader SetCumulativeDifficulty(ulong cumulativeDifficulty)
        {
            this.cumulativeDifficulty = cumulativeDifficulty;
            return this;
        }

        public uint GetMajorVersion()
        {
            return majorVersion;
        }

        public MoneroBlockHeader SetMajorVersion(uint majorVersion)
        {
            this.majorVersion = majorVersion;
            return this;
        }

        public uint GetMinorVersion()
        {
            return minorVersion;
        }

        public MoneroBlockHeader SetMinorVersion(uint minorVersion)
        {
            this.minorVersion = minorVersion;
            return this;
        }

        public ulong GetNonce()
        {
            return nonce;
        }

        public MoneroBlockHeader SetNonce(ulong nonce)
        {
            this.nonce = nonce;
            return this;
        }

        public string GetMinerTxHash()
        {
            return minerTxHash;
        }

        public MoneroBlockHeader SetMinerTxHash(string minerTxHash)
        {
            this.minerTxHash = minerTxHash;
            return this;
        }

        public uint GetNumTxs()
        {
            return numTxs;
        }

        public MoneroBlockHeader SetNumTxs(uint numTxs)
        {
            this.numTxs = numTxs;
            return this;
        }

        public bool GetOrphanStatus()
        {
            return orphanStatus;
        }

        public MoneroBlockHeader SetOrphanStatus(bool orphanStatus)
        {
            this.orphanStatus = orphanStatus;
            return this;
        }

        public string GetPrevHash()
        {
            return prevHash;
        }

        public MoneroBlockHeader SetPrevHash(string prevHash)
        {
            this.prevHash = prevHash;
            return this;
        }

        public ulong GetReward()
        {
            return reward;
        }

        public MoneroBlockHeader SetReward(ulong reward)
        {
            this.reward = reward;
            return this;
        }

        public string GetPowHash()
        {
            return powHash;
        }

        public MoneroBlockHeader SetPowHash(string powHash)
        {
            this.powHash = powHash;
            return this;
        }
    }
}
