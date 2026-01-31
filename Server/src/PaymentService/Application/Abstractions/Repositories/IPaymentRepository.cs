using Application.Abstractions.Repositories.Base;
using Domain.Models;

namespace Application.Abstractions.Repositories
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        ValueTask<Payment> GetByIdAsync(Guid id, CancellationToken ct);
    }
}
