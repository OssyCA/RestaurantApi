using RestaurantApi.DTO;

namespace RestaurantApi.Services.IServices
{
    public interface IAvailabilityService
    {
        Task<List<AvailableTablesDTO>> GetAvailableTablesAsync(DateTime requestStartTime, int amount);
        Task<bool> IsTableAvailableAsync(int tableId, DateTime startAt, int amount, int? excludeBookingId = null);
    }
}
