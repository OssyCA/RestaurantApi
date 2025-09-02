using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApi.DTO;
using RestaurantApi.DTO.Common;
using RestaurantApi.Models;
using RestaurantApi.Services.IServices;

namespace RestaurantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController(ITableService service) : ControllerBase
    {
        [HttpPost("CreateTable")]
        [Authorize(Roles = nameof(EmployeeRole.Admin))]
        public async Task<ActionResult<ApiResponse<int>>> CreateTable(CreateTableDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<int>.Error("Invalid table data"));

            var result = await service.CreateTableAsync(request.TableNumber, request.Capacity);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetTable), new { id = result.Data }, result);
        }

        [HttpGet("GetAllTables")]
        public async Task<ActionResult<ApiResponse<List<TableDTO>>>> GetAllTables()
        {
            var tables = await service.GetAllTablesAsync();
            return Ok(ApiResponse<List<TableDTO>>.Ok(tables, $"Retrieved {tables.Count} tables"));
        }

        [HttpGet("GetTable/{id}")]
        public async Task<ActionResult<ApiResponse<TableDTO>>> GetTable(int id)
        {
            var table = await service.GetTableAsync(id);

            if (table == null)
                return NotFound(ApiResponse<TableDTO>.Error("Table not found"));

            return Ok(ApiResponse<TableDTO>.Ok(table, "Table retrieved successfully"));
        }

        [HttpPut("UpdateTable/{id}")]
        [Authorize(Roles = nameof(EmployeeRole.Admin))]
        public async Task<ActionResult<ApiResponse>> UpdateTable(int id, UpdateTableDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.Error("Invalid update data"));

            var (Success, ErrorMessages) = await service.UpdateTableAsync(id, request);

            if (!Success)
                return BadRequest(ApiResponse.Error("Table update failed", ErrorMessages));

            return Ok(ApiResponse.Ok("Table updated successfully"));
        }

        [HttpDelete("DeleteTable/{id}")]
        [Authorize(Roles = nameof(EmployeeRole.Admin))]
        public async Task<ActionResult<ApiResponse>> DeleteTable(int id)
        {
            var (Success, ErrorMessages) = await service.DeleteTableAsync(id);

            if (!Success)
                return NotFound(ApiResponse.Error("Table deletion failed", ErrorMessages));

            return Ok(ApiResponse.Ok("Table deleted successfully"));
        }
    }
}