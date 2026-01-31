using Application.Abstractions.Services;
using Application.Abstractions.Services.UnitOfWork;
using Infrastructure.Data;

namespace Infrastructure.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly PaymentContext _context;
    
    public UnitOfWork(PaymentContext context)
    {
        _context = context ??  throw new ArgumentNullException(nameof(context));
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return _context.SaveChangesAsync(ct);
    }
}