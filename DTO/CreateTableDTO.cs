using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.DTO
{
    public class CreateTableDTO
    {
        [Required]
        public int TableNumber { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "Capacity must be between 1 and 20")]
        public int Capacity { get; set; }
    }
}
