using System.Text.Json.Serialization;

namespace Monero.Wallet.Rpc;

public class RpcAddressInfo
{
    [JsonPropertyName("address")] public string Address { get; set; } = "";
    [JsonPropertyName("address_index")] public uint AddressIndex { get; set; }
    [JsonPropertyName("label")] public string Label { get; set; } = "";
    [JsonPropertyName("used")] public bool Used { get; set; }
}