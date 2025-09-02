using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Models
{
    public class RestaurantTable
    {
        [Key]
        public int TableId { get; set; }
        [Required]
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public bool BookingLocked { get; set; } = false;  // When u start a booking later with frontend
        public List<Booking> Bookings { get; set; } = [];
    }
}
