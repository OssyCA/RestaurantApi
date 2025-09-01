using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        [ForeignKey("RestaurantTable")]
        public int RestaurantTableId { get; set; }
        public RestaurantTable RestaurantTable { get; set; } = null!;
        public DateTime BookedAt { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? ExpireAt { get; set; }
        public int Amount { get; set; } // Amount of People
    }
}
