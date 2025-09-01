namespace RestaurantApi.DTO.MenuDTOs
{
    public class UpdateMenuItemDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public decimal? Price { get; set; }
        public bool? IsPopular { get; set; }
    }

}
