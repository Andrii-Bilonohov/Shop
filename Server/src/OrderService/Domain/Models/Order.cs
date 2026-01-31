using Domain.Enums;

namespace Domain.Models;

public class Order : BaseModel
{
    public Guid UserId { get; private set; }

    public string Title { get; private set; }
    public string Description { get; private set; }

    public decimal TotalPrice { get; private set; }
    public int TotalItems { get; private set; }

    public OrderStatus Status { get; private set; } = OrderStatus.Created;

    protected Order() { }

    public Order(Guid userId, string title, string description, decimal totalPrice, int totalItems)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty");

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required");

        if (totalPrice <= 0)
            throw new ArgumentException("TotalPrice must be greater than zero");

        if (totalItems <= 0)
            throw new ArgumentException("TotalItems must be greater than zero");

        UserId = userId;
        Title = title;
        Description = description;
        TotalPrice = totalPrice;
        TotalItems = totalItems;
        Status = OrderStatus.Created;

        Touch();
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Order cannot be paid");

        Status = OrderStatus.Paid;

        Touch();
    }

    public void Ship()
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOperationException("Order must be paid before shipping");

        Status = OrderStatus.Shipped;

        Touch();
    }

    public void Complete()
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOperationException("Order must be shipped");

        Status = OrderStatus.Completed;

        Touch();
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Completed order cannot be cancelled");

        Status = OrderStatus.Cancelled;
        
        Touch();
    }

    public void Remove()
    {
        Delete();

        Touch();
    }

    protected override void Delete()
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Completed order cannot be deleted");

        base.Delete();
    }
}