using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.DTO.MenuDTOs
{
    public class MenuItemDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = null!;
        [MaxLength(200)]
        [Required]
        public string Description { get; set; } = null!;
        public bool IsPopular { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}
