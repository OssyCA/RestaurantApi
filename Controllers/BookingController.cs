using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApi.DTO;
using RestaurantApi.DTO.BookingDTOs;
using RestaurantApi.DTO.Common;
using RestaurantApi.Models;
using RestaurantApi.Services;
using RestaurantApi.Services.IServices;

namespace RestaurantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController(IBookingService service, IAvailabilityService availability) : ControllerBase
    {
        [HttpPost("CreateBooking")]
        public async Task<ActionResult<ApiResponse<BookingResponseDTO>>> CreateBooking(BookingDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<BookingResponseDTO>.Error("Invalid booking data"));

            var booking = await service.CreateBooking(request);

            if (!booking.Success)
            {
                return BadRequest(ApiResponse<BookingResponseDTO>.Error(
                    "Already booked or invalid data", booking.Errors));
            }
            if (booking.Data == null)
            {
                return BadRequest(ApiResponse<BookingResponseDTO>.Error("No booking data available"));
            }

            var response = new BookingResponseDTO
            {
                Email = booking.Data.Customer.Email,
                BookingId = booking.Data.BookingId,
                StartAt = booking.Data.StartAt,
                ExpireAt = booking.Data.ExpireAt,
                TableId = booking.Data.RestaurantTableId
            };

            return Ok(ApiResponse<BookingResponseDTO>.Ok(response, "Booking created successfully"));

        }

        [HttpGet("AllBookings")]
        [Authorize(Roles = nameof(EmployeeRole.Admin))]
        public async Task<ActionResult<ApiResponse<List<AllBookingDTO>>>> GetAllBooking()
        {
            var bookings = await service.GetAllBookings();
            return Ok(ApiResponse<List<AllBookingDTO>>.Ok(bookings, $"Retrieved {bookings.Count} bookings"));
        }

        [HttpGet("GetBooking/{id}")]
        [Authorize(Roles = nameof(EmployeeRole.Admin))]
        public async Task<ActionResult<ApiResponse<BookingDTO>>> GetBooking(int id)
        {
            var booking = await service.GetBooking(id);

            if (booking == null)
                return NotFound(ApiResponse<BookingDTO>.Error("Booking not found"));

            return Ok(ApiResponse<BookingDTO>.Ok(booking, "Booking retrieved successfully"));
        }

        [HttpPut("Updatebooking/{id}")]
        [Authorize(Roles = nameof(EmployeeRole.Admin))]
        public async Task<ActionResult<ApiResponse>> UpdateBooking(int id, UpdateBookingDTO request)
        {
            var (Success, ErrorMessages) = await service.UpdateBooking(id, request);

            if (!Success)
                return BadRequest(ApiResponse.Error("Booking update failed", ErrorMessages));

            return Ok(ApiResponse.Ok("Booking updated successfully"));
        }

        [HttpDelete("DeleteBooking/{id}")]
        [Authorize(Roles = nameof(EmployeeRole.Admin))]
        public async Task<ActionResult<ApiResponse>> DeleteBooking(int id)
        {
            var (Success, ErrorMessages) = await service.DeleteBooking(id);

            if (!Success)
                return NotFound(ApiResponse.Error("Booking deletion failed", ErrorMessages));

            return Ok(ApiResponse.Ok("Booking deleted successfully"));
        }

        [HttpGet("GetAvailableTables")]
        public async Task<ActionResult<ApiResponse<List<AvailableTablesDTO>>>> GetAvailableTables(DateTime startTime, int amount)
        {
            var tables = await availability.GetAvailableTablesAsync(startTime, amount);
            return Ok(ApiResponse<List<AvailableTablesDTO>>.Ok(tables, $"Found {tables.Count} available tables"));
        }
    }
}