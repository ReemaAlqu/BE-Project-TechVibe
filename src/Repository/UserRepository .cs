using Microsoft.EntityFrameworkCore;
using src.Database;
using src.Entity;
using src.Utils;

namespace src.Repository
{
    public class UserRepository
    {
        protected DbSet<User> _user;
        protected DatabaseContext _databaseContext;

        public UserRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _user = databaseContext.Set<User>();
        }

        public async Task<User> CreateOneAsync(User newUser)
        {
            await _user.AddAsync(newUser);
            await _databaseContext.SaveChangesAsync();
            return newUser;
        }

        // get all original method
        public async Task<List<User>> GetAllAsync()
        {
            return await _user.ToListAsync();
        }

      

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _user.FindAsync(id);
        }

        public async Task<bool> DeleteOneAsync(User user)
        {
            _user.Remove(user);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOneAsync(User updateUser)
        {
            _user.Update(updateUser);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _user.FirstOrDefaultAsync(u => u.EmailAddress == email);
        }
    }
}
