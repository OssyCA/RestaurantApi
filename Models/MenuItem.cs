using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = null!;
        [MaxLength(200)]
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 9999.99)]
        public decimal Price { get; set; }
        public bool IsPopular { get; set; }
        public string? ImageUrl { get; set; }
    }
}
