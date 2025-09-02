using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantApi.DTO;
using RestaurantApi.Models;

namespace RestaurantApi.Repositories.IRepositories
{
    public interface ITableRepository
    {
        Task<RestaurantTable?> GetTableAsync(int id);
        Task<List<RestaurantTable>> GetAllTables();
        Task<int> CreateTable(RestaurantTable table);
        Task<bool> DeleteTable(int id);
        Task<RestaurantTable?> UpdateTableAsync(int id, UpdateTableDTO dto);
    }
}
