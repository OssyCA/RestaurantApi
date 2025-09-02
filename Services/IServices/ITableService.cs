using RestaurantApi.DTO;
using RestaurantApi.DTO.Common;

namespace RestaurantApi.Services.IServices
{
    public interface ITableService
    {
        Task<ApiResponse<int>> CreateTableAsync(int tableNumber, int capacity);
        Task<List<TableDTO>> GetAllTablesAsync();
        Task<TableDTO?> GetTableAsync(int id);
        Task<(bool Success, List<string> ErrorMessages)> UpdateTableAsync(int id, UpdateTableDTO dto);
        Task<(bool Success, List<string> ErrorMessages)> DeleteTableAsync(int id);
    }
}
