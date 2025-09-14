using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.DTO.BookingDTOs;
using RestaurantApi.DTO.Common;
using RestaurantApi.Models;
using RestaurantApi.Repositories.IRepositories;
using RestaurantApi.Services.IServices;

namespace RestaurantApi.Services
{
    public class BookingService(RestaurantDbContext context, IBookingRepository repository, ITableRepository tableRepository, IAvailabilityService availabilityService): IBookingService
    {

        public async Task<ApiResponse<Booking>> CreateBooking(BookingDTO request)
        {
            if (await availabilityService.IsTableAvailableAsync(request.TableId, request.StartAt, request.Amount))
            {
                var booking = await repository.CreateBooking(request);
                return new ApiResponse<Booking>
                {
                    Success = true,
                    Data = booking
                };
            }
            return new ApiResponse<Booking>
            {
                Success = false,
                Message = "Table is not available for the requested time"
            };

        }
        public async Task<List<AllBookingDTO>> GetAllBookings()
        {
           var bookings = await repository.GetAllBookings();

            var bookingDTOs = bookings.Select(b => new AllBookingDTO
            {
                Id = b.BookingId,
                Amount = b.Amount,
                TableNumber = b.RestaurantTable.TableNumber,
                TableId = b.RestaurantTableId,
                StartTime = b.StartAt,
                EndTime = b.StartAt.AddHours(2),
                CustomerName = b.Customer.Name,
                CustomerEmail = b.Customer.Email,
                CustomerPhone = b.Customer.Phone
            }).ToList();

            return bookingDTOs;
        }
        public async Task<BookingDTO?> GetBooking(int id)
        {
            var booking = await repository.GetBooking(id);
            return booking;
        }
        public async Task<(bool Success, List<string> ErrorMessages)> UpdateBooking(int id, UpdateBookingDTO request)
        {
            var existingBooking = await repository.GetBooking(id);
            if (existingBooking == null)
            {
                var errorMessage = new List<string> { "Booking not found" };
                return (false, errorMessage);
            }
            if (request.TableId.HasValue)
            {
                var table = await tableRepository.GetTableAsync(request.TableId.Value);
                if (table == null)
                {
                    var errorMessage = new List<string> { "Table not found" };
                    return (false, errorMessage);
                }
            }

            if (request.TableId.HasValue || request.StartAt.HasValue)
            {
                var tableIdToCheck = request.TableId ?? existingBooking.TableId;
                var startTimeToCheck = request.StartAt ?? existingBooking.StartAt;
                var amountToCheck = request.Amount ?? existingBooking.Amount;

                if (!await availabilityService.IsTableAvailableAsync(
                    tableIdToCheck, startTimeToCheck, amountToCheck, id)) 
                {
                    var errorMessage = new List<string> { "Table not available for the requested time" };
                    return (false, errorMessage);
                }
            }

            var updatedBooking = await repository.UpdateBooking(id, request);
            if (updatedBooking != null)
            {
                return (true, new List<string>());
            }

            return (false, new List<string> { "Failed to update booking" });
        }
        public async Task<(bool Success, List<string> ErrorMessages)> DeleteBooking(int id)
        {
            var booking = await context.Bookings.FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null)
            {
                var errorMessage = new List<string> { "Can't find booking" };
                return (false, errorMessage);
            }

            var success = await repository.DeleteBooking(id); // Använd id direkt
            if (success)
            {
                return (true, new List<string>());
            }

            return (false, new List<string> { "Failed to delete booking" });
        }

    }
}
