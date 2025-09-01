using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantApi.DTO;
using RestaurantApi.Models;

namespace RestaurantApi.Repositories.IRepositories
{
    public interface ITableRepository
    {
        Task<RestaurantTable> GetTableAsync(int id);
        Task<List<RestaurantTable>> GetAllTables();
    }
}
