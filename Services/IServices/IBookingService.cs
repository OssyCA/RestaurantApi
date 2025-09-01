using RestaurantApi.DTO.BookingDTOs;
using RestaurantApi.Models;

namespace RestaurantApi.Services.IServices
{
    public interface IBookingService
    {
        Task<Booking> CreateBooking(BookingDTO request);
        Task<List<AllBookingDTO>> GetAllBookings();
        Task<BookingDTO> GetBooking(int id);
        Task<(bool Success,  List<string> ErrorMessages)> UpdateBooking(int id, UpdateBookingDTO request);
        Task<(bool Success, List<string> ErrorMessages)> DeleteBooking(int id);
    }
}
