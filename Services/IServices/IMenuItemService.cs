using RestaurantApi.DTO.MenuDTOs;

namespace RestaurantApi.Services.IServices
{
    public interface IMenuItemService
    {
        Task<int> CreateMenuItem(MenuItemDTO dto);
        Task<List<MenuItemDTO>> GetAllMenuItemsAsync();
        Task<MenuItemDTO?> UpdateMenuItemAsync(int id, UpdateMenuItemDTO dto);
        Task<bool> DeleteMenuItemAsync(int id);
        Task<GetMenuItemDTO?> GetMenuItemByIdAsync(int id);
    }
}
