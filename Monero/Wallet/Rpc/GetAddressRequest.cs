using System.Text.Json.Serialization;

namespace Monero.Wallet.Rpc;

public class GetAddressRequest
{
    [JsonPropertyName("account_index")] public uint AccountIndex { get; set; }
    [JsonPropertyName("address_index")] public List<uint>? AddressIndex { get; set; }
}