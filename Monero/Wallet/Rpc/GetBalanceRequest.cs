using System.Text.Json.Serialization;

namespace Monero.Wallet.Rpc;

public class GetBalanceRequest
{
    [JsonPropertyName("account_index")] public uint AccountIndex { get; set; }
    [JsonPropertyName("address_indices")] public List<uint>? AddressIndices { get; set; }
    [JsonPropertyName("all_accounts")] public bool AllAccounts { get; set; }
    [JsonPropertyName("strict")] public bool Strict { get; set; }
}