using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.DTO;
using RestaurantApi.Models;
using RestaurantApi.Repositories.IRepositories;

namespace RestaurantApi.Repositories
{
    public class TableRepository(RestaurantDbContext context) : ITableRepository
    {
        public async Task<RestaurantTable> GetTableAsync(int id)
        {
            var table = await context.RestaurantTables.FirstOrDefaultAsync(t => t.TableId == id);

            if (table == null)
            {
                return null;
            }

            return table;
        }
        public async Task<List<RestaurantTable>> GetAllTables()
        {
            return await context.RestaurantTables.Include(t => t.Bookings).ToListAsync();
        }
    }
}
