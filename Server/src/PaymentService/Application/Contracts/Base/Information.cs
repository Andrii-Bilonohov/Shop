using System.Text.Json.Serialization;

namespace Application.Contracts.Base;

public record Information(
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Error = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    Guid? Id = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Message = null
)
{
    [property: JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public bool Success => Error is null;
}
