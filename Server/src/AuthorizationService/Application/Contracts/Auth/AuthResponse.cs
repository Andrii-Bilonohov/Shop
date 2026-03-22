using System.Text.Json.Serialization;

namespace Application.Contracts.Auth
{
    public record AuthResponse(
        [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        string? Error = null,

        [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        string? AccessToken = null
    )
    {
        [JsonIgnore]
        public bool Success => Error == null;
    }
}
