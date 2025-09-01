using RestaurantApi.DTO.BookingDTOs;
using RestaurantApi.Models;

namespace RestaurantApi.Repositories.IRepositories
{
    public interface IBookingRepository
    {
        Task<Booking> CreateBooking(BookingDTO request);
        Task<List<Booking>> GetAllBookings();
        Task<BookingDTO> GetBooking(int id);
        Task<UpdateBookingDTO> UpdateBooking(int id, UpdateBookingDTO request);
        Task<bool> DeleteBooking(int id);
        Task<bool> HasBookingConflictAsync(int tableId, DateTime startAt, DateTime endAt, int? excludeBookingId = null);

    }
}
