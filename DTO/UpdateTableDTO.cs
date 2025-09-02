using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.DTO
{
    public class UpdateTableDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Table number must be greater than 0")]
        public int? TableNumber { get; set; }

        [Range(1, 20, ErrorMessage = "Capacity must be between 1 and 20")]
        public int? Capacity { get; set; }
        public bool? BookingLocked { get; set; }
    }
}
