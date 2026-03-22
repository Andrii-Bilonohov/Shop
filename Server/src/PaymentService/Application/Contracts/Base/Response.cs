using System.Text.Json.Serialization;

namespace Application.Contracts.Base
{
    public record Response<T>
    (
        [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        string? Error = null,
        [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        T? Data = default
    )
    {
        [JsonIgnore]
        public bool Success => Error is null;
    }

    public record ResponseList<T>
    (
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Error = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    int? Limit = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    int? Offset = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    int? Items = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    int? Pages = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    IEnumerable<T>? Data = null
    )
    {
        [JsonIgnore]
        public bool Success => Error is null;
    }
}
