using Application.Abstractions.Services;
using Application.Abstractions.Services.UnitOfWork;
using Infrastructure.Data;

namespace Infrastructure.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly ItemContext _context;
    
    public UnitOfWork(ItemContext context)
    {
        _context = context;
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}