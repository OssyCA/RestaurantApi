using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.DTO
{
    public class EmployeeDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Required]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        public string Password { get; set; } = string.Empty;
    }
}
