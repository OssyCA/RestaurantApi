using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.DTO.MenuDTOs;
using RestaurantApi.Models;
using RestaurantApi.Repositories.IRepositories;

namespace RestaurantApi.Repositories
{
    public class MenuItemRepositrory(RestaurantDbContext context) : IMenuItemRepository
    {
        public async Task<int> CreateMenuItem(MenuItem menyItem)
        {
            context.MenuItems.Add(menyItem);
            await context.SaveChangesAsync();
            return menyItem.Id;
        }
        public async Task<bool> DeleteMenuItem(int id)
        {
            var deletedRows = await context.MenuItems
                .Where(b => b.Id == id)
                .ExecuteDeleteAsync();

            return deletedRows > 0;

        }

        public async Task<List<MenuItemDTO>> GetAllMenuItems()
        {
            var menus = await context.MenuItems.ToListAsync();

            if (menus.Count == 0) return [];

            var wholeMenu = menus.Select(menu => new MenuItemDTO
            {
                Id = menu.Id,   
                Title = menu.Title,
                Description = menu.Description,
                ImageUrl = menu.ImageUrl,
                Price = menu.Price,
                IsPopular = menu.IsPopular
            }).ToList();

            return wholeMenu;
        }

        public async Task<MenuItem?> GetMenuItemById(int id)
        {
            return await context.MenuItems
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<MenuItemDTO?> UpdateMenuItemAsync(int id, UpdateMenuItemDTO dto)
        {
            var menuItem = await context.MenuItems.FirstOrDefaultAsync(menu => menu.Id == id);

            if (menuItem == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(dto.Title)) menuItem.Title = dto.Title.Trim();
            if (!string.IsNullOrWhiteSpace(dto.Description)) menuItem.Description = dto.Description; 
            if (!string.IsNullOrWhiteSpace(dto.ImageUrl)) menuItem.ImageUrl = dto.ImageUrl;          
            if (dto.Price.HasValue) menuItem.Price = dto.Price.Value;
            if (dto.IsPopular.HasValue) menuItem.IsPopular = dto.IsPopular.Value;

            await context.SaveChangesAsync();

            return new MenuItemDTO
            {
                Title = menuItem.Title,
                Description = menuItem.Description,
                ImageUrl = menuItem.ImageUrl,
                Price = menuItem.Price,
                IsPopular = menuItem.IsPopular
            };
        }

    }
}
