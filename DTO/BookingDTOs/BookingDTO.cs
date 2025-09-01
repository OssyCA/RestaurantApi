using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.DTO.BookingDTOs
{
    public class BookingDTO
    {
        public int TableId { get; set; }
        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;
        [Phone]
        public string CustomerPhone { get; set; } = string.Empty;
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;
        public int Amount { get; set; }
        public DateTime StartAt { get; set; }
    }
}
