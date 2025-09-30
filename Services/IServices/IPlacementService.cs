using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantApi.Models;

namespace RestaurantApi.Services.IServices
{
    public interface IPlacementService
    {
        Task<RestaurantTable> AssignTable(DateTime requestStartTime, int amount); 
    }
}
