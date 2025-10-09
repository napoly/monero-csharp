using System.Text.Json.Serialization;

namespace Monero.Wallet.Rpc;

public class CreateAccountResponse
{
    [JsonPropertyName("account_index")]
    public uint AccountIndex { get; set; }

    [JsonPropertyName("address")]
    public string? Address { get; set; }
}