using RestaurantApi.DTO.MenuDTOs;
using RestaurantApi.Models;
using RestaurantApi.Repositories.IRepositories;
using RestaurantApi.Services.IServices;

namespace RestaurantApi.Services
{
    public class MenuItemService(IMenuItemRepository itemRepository): IMenuItemService
    {
        public async Task<int> CreateMenuItem(MenuItemDTO dto)
        {
            var item = new MenuItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                IsPopular = dto.IsPopular
            };

            var newItem = await itemRepository.CreateMenuItem(item);

            return newItem;
        }

        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            return await itemRepository.DeleteMenuItem(id);
        }

        public async Task<List<MenuItemDTO>> GetAllMenuItemsAsync()
        {
            return await itemRepository.GetAllMenuItems() ?? new List<MenuItemDTO>();
        }

        public async Task<GetMenuItemDTO?> GetMenuItemByIdAsync(int id)
        {
            var menuItem = await itemRepository.GetMenuItemById(id);
            if (menuItem == null)
            {
                return null;
            }
            return new GetMenuItemDTO
            {
                Title = menuItem.Title,
                Description = menuItem.Description,
                Price = menuItem.Price,
                ImageUrl = menuItem.ImageUrl,
                IsPopular = menuItem.IsPopular
            };
        }

        public async Task<MenuItemDTO?> UpdateMenuItemAsync(int id, UpdateMenuItemDTO dto)
        {
            return await itemRepository.UpdateMenuItemAsync(id, dto);
        }
    }
}
