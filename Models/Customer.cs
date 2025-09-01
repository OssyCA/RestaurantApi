using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
        [EmailAddress]
        [Required]
        public string Email { get; set; } =null!;
        [Phone]
        [Required]
        public string Phone { get; set; } = null!;
    }
}
