using System.Text.Json.Serialization;

namespace Monero.Wallet.Rpc;

public class GetAddressResponse
{
    [JsonPropertyName("address")] public string Address { get; set; } = "";
    [JsonPropertyName("addresses")] public List<RpcAddressInfo> Addresses { get; set; } = [];
}