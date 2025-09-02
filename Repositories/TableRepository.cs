using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.DTO;
using RestaurantApi.Models;
using RestaurantApi.Repositories.IRepositories;

namespace RestaurantApi.Repositories
{
    public class TableRepository(RestaurantDbContext context) : ITableRepository
    {
        public async Task<RestaurantTable?> GetTableAsync(int id)
        {
            return await context.RestaurantTables
                .FirstOrDefaultAsync(t => t.TableId == id);
        }
        public async Task<List<RestaurantTable>> GetAllTables()
        {
            return await context.RestaurantTables.Include(t => t.Bookings).ToListAsync();
        }
        public async Task<int> CreateTable(RestaurantTable table)
        {
            context.RestaurantTables.Add(table);
            await context.SaveChangesAsync();

            return table.TableId;
        }

        public Task<bool> DeleteTable(int id)
        {
            throw new NotImplementedException();
        }

        public Task<RestaurantTable?> UpdateTableAsync(int id, UpdateTableDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
