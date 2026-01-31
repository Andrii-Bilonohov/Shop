using Application.Abstractions.Repositories;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories
{
    public class AuthRepository : BaseRepository<User>, IAuthRepository
    {
        public AuthRepository(UserContext context) : base(context) { }
    }
}
