
namespace Monero.Daemon.Common
{
    public class MoneroSubmitTxResult
    {
        private bool isGood;
        private bool isRelayed;
        private bool isDoubleSpend;
        private bool isFeeTooLow;
        private bool isMixinTooLow;
        private bool hasInvalidInput;
        private bool hasInvalidOutput;
        private bool hasTooFewOutputs;
        private bool isOverspend;
        private bool isTooBig;
        private bool sanityCheckFailed;
        private string reason;
        private ulong credits;
        private string topBlockHash;
        private bool isTxExtraTooBig;
        private bool isNonzeroUnlockTime;

        public bool IsGood()
        {
            return isGood;
        }

        public void SetIsGood(bool isGood)
        {
            this.isGood = isGood;
        }

        public bool IsRelayed()
        {
            return isRelayed;
        }

        public void SetIsRelayed(bool isRelayed)
        {
            this.isRelayed = isRelayed;
        }

        public bool IsDoubleSpend()
        {
            return isDoubleSpend;
        }

        public void SetIsDoubleSpend(bool isDoubleSpend)
        {
            this.isDoubleSpend = isDoubleSpend;
        }

        public bool IsFeeTooLow()
        {
            return isFeeTooLow;
        }

        public void SetIsFeeTooLow(bool isFeeTooLow)
        {
            this.isFeeTooLow = isFeeTooLow;
        }

        public bool IsMixinTooLow()
        {
            return isMixinTooLow;
        }

        public void SetIsMixinTooLow(bool isMixinTooLow)
        {
            this.isMixinTooLow = isMixinTooLow;
        }

        public bool HasInvalidInput()
        {
            return hasInvalidInput;
        }

        public void SetHasInvalidInput(bool hasInvalidInput)
        {
            this.hasInvalidInput = hasInvalidInput;
        }

        public bool HasInvalidOutput()
        {
            return hasInvalidOutput;
        }

        public void SetHasInvalidOutput(bool hasInvalidOutput)
        {
            this.hasInvalidOutput = hasInvalidOutput;
        }

        public bool HasTooFewOutputs()
        {
            return hasTooFewOutputs;
        }

        public void SetHasTooFewOutputs(bool hasTooFewOutputs)
        {
            this.hasTooFewOutputs = hasTooFewOutputs;
        }

        public bool IsOverspend()
        {
            return isOverspend;
        }

        public void SetIsOverspend(bool isOverspend)
        {
            this.isOverspend = isOverspend;
        }

        public bool IsTooBig()
        {
            return isTooBig;
        }

        public void SetIsTooBig(bool isTooBig)
        {
            this.isTooBig = isTooBig;
        }

        public bool GetSanityCheckFailed()
        {
            return sanityCheckFailed;
        }

        public void SetSanityCheckFailed(bool sanityCheckFailed)
        {
            this.sanityCheckFailed = sanityCheckFailed;
        }

        public string GetReason()
        {
            return reason;
        }

        public void SetReason(string reason)
        {
            this.reason = reason;
        }

        public ulong GetCredits()
        {
            return credits;
        }

        public void SetCredits(ulong credits)
        {
            this.credits = credits;
        }

        public string GetTopBlockHash()
        {
            return topBlockHash;
        }

        public void SetTopBlockHash(string topBlockHash)
        {
            this.topBlockHash = topBlockHash;
        }

        public bool IsTxExtraTooBig()
        {
            return isTxExtraTooBig;
        }

        public void SetIsTxExtraTooBig(bool isTxExtraTooBig)
        {
            this.isTxExtraTooBig = isTxExtraTooBig;
        }
        
        public bool IsNonzeroUnlockTime()
        {
            return isNonzeroUnlockTime;
        }

        public void SetIsNonzeroUnlockTime(bool isNonzeroUnlockTime)
        {
            this.isNonzeroUnlockTime = isNonzeroUnlockTime;
        }
    }
}
