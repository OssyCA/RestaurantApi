using RestaurantApi.DTO.BookingDTOs;
using RestaurantApi.DTO.MenuDTOs;
using RestaurantApi.Models;

namespace RestaurantApi.Repositories.IRepositories
{
    public interface IMenuItemRepository
    {
        Task<int> CreateMenuItem(MenuItem menyItem);
        Task<MenuItemDTO?> UpdateMenuItemAsync(int id, UpdateMenuItemDTO dto);
        Task<bool> DeleteMenuItem(int id);
        Task<List<MenuItemDTO>> GetAllMenuItems();

    }
}
