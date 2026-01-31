using Domain.Enums;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Application.Filters
{
    public record ItemFilter
    (
        [property: JsonConverter(typeof(StringEnumConverter))]
        Category? Category = null
    );
}
