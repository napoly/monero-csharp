
namespace Monero.Common
{
    public abstract class MoneroNetwork
    {
        private readonly int primaryAddressCode;
        private readonly int integratedAddressCode;
        private readonly int subaddressCode;

        public MoneroNetwork(int primaryAddressCode, int integratedAddressCode, int subaddressCode)
        {
            this.primaryAddressCode = primaryAddressCode;
            this.integratedAddressCode = integratedAddressCode;
            this.subaddressCode = subaddressCode;
        }

        public int GetPrimaryAddressCode()
        {
            return primaryAddressCode;
        }

        public int GetIntegratedAddressCode()
        {
            return integratedAddressCode;
        }

        public int GetSubaddressCode()
        {
            return subaddressCode;
        }

        public static MoneroNetworkType Parse(string? networkTypeStr)
        {
            if (networkTypeStr == null) throw new MoneroError("Cannot parse null network type");
            return networkTypeStr.ToLower() switch
            {
                "mainnet" => MoneroNetworkType.MAINNET,
                "testnet" => MoneroNetworkType.TESTNET,
                "stagenet" => MoneroNetworkType.STAGENET,
                _ => throw new MoneroError("Invalid network type to parse: " + networkTypeStr),
            };
        }
    }

    public class MoneroNetworkMainnet : MoneroNetwork
    {
        public MoneroNetworkMainnet(): base(18, 19, 42) { }
    }

    public class MoneroNetworkTestnet : MoneroNetwork
    {
        public MoneroNetworkTestnet() : base(53, 54, 63) { }
    }

    public class MoneroNetworkStagenet : MoneroNetwork
    {
        public MoneroNetworkStagenet() : base(24, 25, 36) { }
    }
}
