using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantApi.DTO;
using RestaurantApi.DTO.Common;
using RestaurantApi.DTO.MenuDTOs;
using RestaurantApi.Models;
using RestaurantApi.Services.IServices;

namespace RestaurantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController(IMenuItemService service) : ControllerBase
    {
        [HttpPost("CreateMenuItem")]
        [Authorize(Roles = nameof(EmployeeRole.Admin))]
        public async Task<ActionResult<ApiResponse<int>>> CreateItem(MenuItemDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<int>.Error("Invalid menu item data"));

            var newId = await service.CreateMenuItem(dto);

            return CreatedAtAction(nameof(GetAll), new { id = newId },
                ApiResponse<int>.Ok(newId, "Menu item created successfully"));
        }

        [HttpPut("UpdateItem/{id}")]
        [Authorize(Roles = nameof(EmployeeRole.Admin))]
        public async Task<ActionResult<ApiResponse<MenuItemDTO>>> UpdateItem(int id, UpdateMenuItemDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<MenuItemDTO>.Error("Invalid update data"));

            var updated = await service.UpdateMenuItemAsync(id, dto);

            if (updated is null)
                return NotFound(ApiResponse<MenuItemDTO>.Error("Menu item not found"));

            return Ok(ApiResponse<MenuItemDTO>.Ok(updated, "Menu item updated successfully"));
        }

        [HttpGet("GetWholeMenu")]
        public async Task<ActionResult<ApiResponse<List<MenuItemDTO>>>> GetAll()
        {
            var items = await service.GetAllMenuItemsAsync();
            return Ok(ApiResponse<List<MenuItemDTO>>.Ok(items, $"Retrieved {items.Count} menu items"));
        }

        [HttpDelete("DeleteItem/{id:int}")]
        [Authorize(Roles = nameof(EmployeeRole.Admin))]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            var ok = await service.DeleteMenuItemAsync(id);

            if (!ok)
                return NotFound(ApiResponse.Error("Menu item not found"));

            return Ok(ApiResponse.Ok("Menu item deleted successfully"));
        }
        [HttpGet("GetItem/{id}")]
        public async Task<ActionResult<ApiResponse<GetMenuItemDTO>>> GetMenuItem(int id)
        {
            var item = await service.GetMenuItemByIdAsync(id);

            if (item == null)
                return NotFound(ApiResponse<GetMenuItemDTO>.Error("Item  not found"));

            return Ok(ApiResponse<GetMenuItemDTO>.Ok(item, "Item retrieved successfully"));

        }
    }
}