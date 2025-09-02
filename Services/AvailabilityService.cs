using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.DTO;
using RestaurantApi.Models;
using RestaurantApi.Repositories;
using RestaurantApi.Repositories.IRepositories;
using RestaurantApi.Services.IServices;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RestaurantApi.Services
{
    public class AvailabilityService(ITableRepository tableRepository, IBookingRepository bookingRepository): IAvailabilityService
    {
        public async Task<List<AvailableTablesDTO>> GetAvailableTablesAsync(DateTime requestStartTime, int amount)
        {
            var freeTables = new List<AvailableTablesDTO>();

            foreach (var table in await tableRepository.GetAllTables())
            {
                if (await IsTableAvailableAsync(table.TableId, requestStartTime, amount))
                {
                    freeTables.Add(new AvailableTablesDTO
                    {
                        TableId = table.TableId,
                        Capacity = table.Capacity,
                    });
                }
            }

            return freeTables;
        }
        public async Task<bool> IsTableAvailableAsync(int tableId, DateTime startAt, int amount, int? excludeBookingId = null)
        {
            var endAt = startAt.AddHours(2);
            var closingTime = new TimeSpan(22, 0, 0);
            var openTime = new TimeSpan(11, 0, 0);

            if (startAt <= DateTime.UtcNow)
                return false;

            var bookingTime = startAt.TimeOfDay;
            if (bookingTime < openTime || bookingTime > closingTime)
                return false;

            var table = await tableRepository.GetTableAsync(tableId);
            if (table is null || table.BookingLocked)
                return false;

            if (amount > table.Capacity)
                return false;

            var hasConflict = await bookingRepository.HasBookingConflictAsync(tableId, startAt, endAt, excludeBookingId);

            return !hasConflict;
        }

    }
}
