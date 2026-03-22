using Domain.Enums;
using System.Text.Json.Serialization;

namespace Application.Filters
{
    public record ItemFilter
    (
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        Category? Category = null
    );
}
