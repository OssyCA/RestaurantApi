using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.DTO.BookingDTOs
{
    public class UpdateBookingDTO
    {
        public int? TableId { get; set; }

        [EmailAddress]
        public string? CustomerEmail { get; set; }

        [Phone]
        public string? CustomerPhone { get; set; }

        [StringLength(100)]
        public string? CustomerName { get; set; }

        public int? Amount { get; set; }

        public DateTime? StartAt { get; set; }
    }
}