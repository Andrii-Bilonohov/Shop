namespace Infrastructure.Options;

public class LiqPayOptions
{
    public string PublicKey { get; init; } = null!;
    public string PrivateKey { get; init; } = null!;
    public string CallbackUrl { get; init; } = null!;
    public string ResultUrl { get; init; } = null!;
}
