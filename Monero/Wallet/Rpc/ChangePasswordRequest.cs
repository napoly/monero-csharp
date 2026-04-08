using System.Text.Json.Serialization;

namespace Monero.Wallet.Rpc;

public class ChangePasswordRequest
{
    [JsonPropertyName("old_password")] public required string OldPassword { get; set; }
    [JsonPropertyName("new_password")] public required string NewPassword { get; set; }
}