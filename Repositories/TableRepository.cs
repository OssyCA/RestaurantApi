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

        public async Task<bool> DeleteTable(int id)
        {
            var deletedRows = await context.RestaurantTables
               .Where(b => b.TableId == id)
               .ExecuteDeleteAsync();

            return deletedRows > 0;
        }

        public async Task<RestaurantTable?> UpdateTableAsync(int id, UpdateTableDTO dto)
        {
            var table = await context.RestaurantTables.FirstOrDefaultAsync(t => t.TableId == id);

            if (table == null)
                return null;

            if (dto.TableNumber.HasValue)
                table.TableNumber = dto.TableNumber.Value;

            if (dto.Capacity.HasValue)
                table.Capacity = dto.Capacity.Value;

            if (dto.BookingLocked.HasValue)
                table.BookingLocked = dto.BookingLocked.Value;

            await context.SaveChangesAsync();
            return table;
        }
    }
}
