namespace RestaurantApi.DTO.MenuDTOs
{
    public class GetMenuItemDTO
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsPopular { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}
