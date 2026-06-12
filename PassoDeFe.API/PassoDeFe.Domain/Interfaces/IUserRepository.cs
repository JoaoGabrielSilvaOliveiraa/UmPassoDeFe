using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassoDeFe.Domain.Entities;
using System.Threading.Tasks;

namespace PassoDeFe.Domain.Interfaces
{
   public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task AddAsync(User user);
    }
}
