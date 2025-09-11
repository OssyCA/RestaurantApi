using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.DTO.BookingDTOs;
using RestaurantApi.Models;
using RestaurantApi.Repositories.IRepositories;

namespace RestaurantApi.Repositories
{
    public class BookingRepository(RestaurantDbContext context, ICustomerRepository customerRepository) : IBookingRepository
    {
        public async Task<Booking?> CreateBooking(BookingDTO request)
        {
            var customer = await customerRepository.GetOrCreateCustomerAsync(
                request.CustomerName,
                request.CustomerEmail,
                request.CustomerPhone);

            if (customer == null)
            {
                return null;
            }

            var booking = new Booking
            {
                CustomerId = customer.CustomerId,
                RestaurantTableId = request.TableId,
                BookedAt = DateTime.UtcNow,
                StartAt = request.StartAt,
                Amount = request.Amount,
            };

            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            return booking;
        }
        public async Task<bool> DeleteBooking(int id)
        {
            var deletedRows = await context.Bookings
                .Where(b => b.BookingId == id)
                .ExecuteDeleteAsync();

            return deletedRows > 0;
        }
        public async Task<List<Booking>> GetAllBookings()
        {
            return await context.Bookings
               .Include(b => b.Customer) 
               .Include(b => b.RestaurantTable)
               .ToListAsync();
        }
        public async Task<BookingDTO?> GetBooking(int id)
        {
            var booking = await context.Bookings
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return null; 
            }


            return new BookingDTO
            {
                TableId = booking.RestaurantTableId,
                CustomerEmail = booking.Customer.Email,
                CustomerPhone = booking.Customer.Phone,
                CustomerName = booking.Customer.Name,
                Amount = booking.Amount,
                StartAt = booking.StartAt
            };
        }
        public async Task<UpdateBookingDTO?> UpdateBooking(int id, UpdateBookingDTO request)
        {
            var booking = await context.Bookings
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) return null;

            if (!string.IsNullOrWhiteSpace(request.CustomerEmail) &&
                booking.Customer.Email != request.CustomerEmail)
            {
                var newCustomer = await customerRepository.GetOrCreateCustomerAsync(
                    request.CustomerName ?? booking.Customer.Name,
                    request.CustomerEmail,
                    request.CustomerPhone ?? booking.Customer.Phone);

                if (newCustomer != null)
                {
                    booking.CustomerId = newCustomer.CustomerId;
                }
            }
            else
            {

                if (!string.IsNullOrWhiteSpace(request.CustomerName))
                    booking.Customer.Name = request.CustomerName.Trim();

                if (!string.IsNullOrWhiteSpace(request.CustomerPhone))
                    booking.Customer.Phone = request.CustomerPhone.Trim();
            }

            if (request.TableId.HasValue)
                booking.RestaurantTableId = request.TableId.Value;

            if (request.Amount.HasValue)
                booking.Amount = request.Amount.Value;

            if (request.StartAt.HasValue)
                booking.StartAt = request.StartAt.Value;

            await context.SaveChangesAsync();
            return request;
        }
        public async Task<bool> HasBookingConflictAsync(int tableId, DateTime startAt, DateTime endAt, int? excludeBookingId = null)
        {
            return await context.Bookings
                .Where(b => b.RestaurantTableId == tableId)
                .Where(b => excludeBookingId == null || b.BookingId != excludeBookingId)
                .Where(b =>
                    startAt < (b.ExpireAt ?? b.StartAt.AddHours(2)) &&
                    endAt > b.StartAt
                )
                .AnyAsync();
        }
    }
}
