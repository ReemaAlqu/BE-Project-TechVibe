using Microsoft.EntityFrameworkCore;
using src.Database;
using src.Entity;

namespace src.Repository
{
    public class AddressRepository
    {
        //create field to access Address table:
        protected DbSet<Address> _address;

        //create field to access the database:

        protected DatabaseContext _databaseContext;

        // DI the DatabaseContext (Database) in the constructer by passing it as parametr value
        public AddressRepository(DatabaseContext databaseContext)
        {
            // Assign the value of the data base in the feild _databaseContext:
            _databaseContext = databaseContext;

            // initialize the Address table in database:
            _address = databaseContext.Set<Address>();
        }

        // methods


        // create a new Address:

        public async Task<Address> CreateOneAsync(Address newAddress)
        {
            await _address.AddAsync(newAddress);
            await _databaseContext.SaveChangesAsync();

            return newAddress;
        }

        // get all Addresses:

        public async Task<List<Address>> GetAllAsync()
        {
            return await _address.ToListAsync();
        }

        // get all Addresses:

        // add the Pagination

        // public async Task<List<Address>> GetAllAsync(PaginationOptions paginationOptions)
        // {
        //     var result = _address.Where(c =>
        //         c.Country.ToLower().Contains(paginationOptions.Search)
        //     );
        //     return await result
        //         .Skip(paginationOptions.Offset)
        //         .Take(paginationOptions.Limit)
        //         .ToListAsync();
        // }

        // get by Address by ID

        public async Task<Address?> GetAddressByIdAsync(Guid id)
        {
            return await _address.FirstOrDefaultAsync(a => a.AddressId == id);
            

        
        }

        //delete Address:

        public async Task<bool> DeleteOneAsync(Address address)
        {
            _address.Remove(address);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        //Update Address:
        public async Task<bool> UpdateOneAsync(Address updatedAddress)
        {
            _address.Update(updatedAddress);
            await _databaseContext.SaveChangesAsync();
            return true;
        }
    }
}
