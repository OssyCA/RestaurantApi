using RestaurantApi.DTO;
using RestaurantApi.DTO.Common;
using RestaurantApi.Models;
using RestaurantApi.Repositories.IRepositories;
using RestaurantApi.Services.IServices;

namespace RestaurantApi.Services
{
    public class TableService(ITableRepository tableRepository) : ITableService
    {
        public async Task<ApiResponse<int>> CreateTableAsync(int tableNumber, int capacity)
        {
            var table = new RestaurantTable
            {
                TableNumber = tableNumber,
                Capacity = capacity,
                BookingLocked = false
            };

            try
            {
                var newTableId = await tableRepository.CreateTable(table);
                return ApiResponse<int>.Ok(newTableId, "Table created successfully");
            }
            catch
            {
                return ApiResponse<int>.Error("Table number already exists or database error");
            }
        }

        public async Task<List<TableDTO>> GetAllTablesAsync()
        {
            var tables = await tableRepository.GetAllTables();

            return [.. tables.Select(t => new TableDTO
            {
                Id = t.TableId,
                TableNumber = t.TableNumber,
                Capacity = t.Capacity
            })];
        }

        public async Task<TableDTO?> GetTableAsync(int id)
        {
            var table = await tableRepository.GetTableAsync(id);

            if (table == null)
                return null;

            return new TableDTO
            {
                Id = table.TableId,
                TableNumber = table.TableNumber,
                Capacity = table.Capacity
            };
        }

        public async Task<(bool Success, List<string> ErrorMessages)> UpdateTableAsync(int id, UpdateTableDTO dto)
        {
            var table = await tableRepository.GetTableAsync(id);
            if (table == null)
            {
                return (false, new List<string> { "Table not found" });
            }

            try
            {
                var updatedTable = await tableRepository.UpdateTableAsync(id, dto);
                if (updatedTable != null)
                {
                    return (true, new List<string>());
                }
                return (false, new List<string> { "Failed to update table" });
            }
            catch
            {
                return (false, new List<string> { "Table number already exists or database error" });
            }
        }

        public async Task<(bool Success, List<string> ErrorMessages)> DeleteTableAsync(int id)
        {
            var table = await tableRepository.GetTableAsync(id);
            if (table == null)
            {
                return (false, new List<string> { "Table not found" });
            }

            var success = await tableRepository.DeleteTable(id);
            if (success)
            {
                return (true, new List<string>());
            }

            return (false, new List<string> { "Failed to delete table" });
        }
    }
}