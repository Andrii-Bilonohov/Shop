namespace Domain.Models;

public class Review : BaseModel
{
    public Guid UserId { get; private set; }
    public Guid ItemId { get; private set; }
    public Item Item { get; private set; } = null!;
    public int Rating { get; private set; }
    public string? Description { get; private set; }
    public string? ImageUrl { get; private set; }

    protected Review() { }

    public Review(Guid userId, Guid itemId, int rating, string? description, string? imageUrl)
    {
        if (userId == Guid.Empty) throw new ArgumentException("UserId is required", nameof(userId));
        if (itemId == Guid.Empty) throw new ArgumentException("ItemId is required", nameof(itemId));

        UserId = userId;
        ItemId = itemId;

        Update(rating, description, imageUrl);
    }

    public void Update(int rating, string? description, string? imageUrl)
    {
        if (rating is < 1 or > 5)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5");

        if (description?.Length > 2000)
            throw new ArgumentException("Description is too long (max 2000)", nameof(description));

        if (!string.IsNullOrWhiteSpace(imageUrl) &&
            !Uri.TryCreate(imageUrl, UriKind.Absolute, out _))
            throw new ArgumentException("ImageUrl is invalid", nameof(imageUrl));

        Rating = rating;
        Description = description?.Trim();
        ImageUrl = string.IsNullOrWhiteSpace(imageUrl) ? null : imageUrl.Trim();

        Touch();
    }
}