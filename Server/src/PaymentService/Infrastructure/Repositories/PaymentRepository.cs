using Application.Abstractions.Repositories;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(PaymentContext context) : base(context) { }
        
        public async ValueTask<Payment?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Id == id, ct);
        }
    }
}