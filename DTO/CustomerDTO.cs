using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.DTO
{
    public class CustomerDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [Phone]
        public string Phone { get; set; } = null!;
    }
}
