namespace Infrastructure.Options
{
    public sealed class GatewayOptions
    {
        public const string SectionName = "Gateway";
        public string HeaderName { get; init; } = "X-Api-Gateway";
        public string Secret { get; init; } = string.Empty;
    }
}
