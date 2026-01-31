using Application.Abstractions.Services;
using Application.Abstractions.Services.UnitOfWork;
using Infrastructure.Data;

namespace Infrastructure.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly OrderContext _context;
    
    public UnitOfWork(OrderContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}