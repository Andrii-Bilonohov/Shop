using Application.Abstractions.Repositories.Base;
using Domain.Models;

namespace Application.Abstractions.Repositories
{
    public interface IAuthRepository : IBaseRepository<User> { }
}
