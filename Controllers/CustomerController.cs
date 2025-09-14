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
    public class CustomerController(ICustomerService service) : ControllerBase
    {
        [HttpPost("CreateCustomer")]
        [Authorize(Roles = nameof(EmployeeRole.Admin))]
        public async Task<ActionResult<ApiResponse<Customer>>> CreateCustomer(CustomerDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<Customer>.Error("Invalid customer data"));

            var result = await service.CreateCustomerAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetCustomerByEmail),
                new { email = result.Data.Email}, result);
        }

        [HttpGet("GetCustomerByEmail/{email}")]
        [Authorize(Roles = nameof(EmployeeRole.Admin))]
        public async Task<ActionResult<ApiResponse<CustomerDTO>>> GetCustomerByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(ApiResponse<CustomerDTO>.Error("Email is required"));

            var customer = await service.GetCustomerByEmailOrPhoneAsync(email, "");

            if (customer == null)
                return NotFound(ApiResponse<CustomerDTO>.Error("Customer not found"));

            return Ok(ApiResponse<CustomerDTO>.Ok(customer, "Customer retrieved successfully"));
        }

        [HttpGet("GetCustomerByPhone{phone}")]
        [Authorize(Roles = nameof(EmployeeRole.Admin))]
        public async Task<ActionResult<ApiResponse<CustomerDTO>>> GetCustomerByPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return BadRequest(ApiResponse<CustomerDTO>.Error("Phone is required"));

            var customer = await service.GetCustomerByEmailOrPhoneAsync("", phone);

            if (customer == null)
                return NotFound(ApiResponse<CustomerDTO>.Error("Customer not found"));

            return Ok(ApiResponse<CustomerDTO>.Ok(customer, "Customer retrieved successfully"));
        }

        [HttpPut("UpdateCustomer/{customerId}")]
        [Authorize(Roles = nameof(EmployeeRole.Admin))]
        public async Task<ActionResult<ApiResponse>> UpdateCustomer(int customerId, CustomerDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.Error("Invalid update data"));

            var (Success, ErrorMessages) = await service.UpdateCustomerAsync(customerId, request);

            if (!Success)
                return BadRequest(ApiResponse.Error("Customer update failed", ErrorMessages));

            return Ok(ApiResponse.Ok("Customer updated successfully"));
        }

        [HttpDelete("DeleteCustomer/{customerId}")]
        [Authorize(Roles = nameof(EmployeeRole.Admin))]
        public async Task<ActionResult<ApiResponse>> DeleteCustomer(int customerId)
        {
            var (Success, ErrorMessages) = await service.DeleteCustomerAsync(customerId);

            if (!Success)
                return NotFound(ApiResponse.Error("Customer deletion failed", ErrorMessages));

            return Ok(ApiResponse.Ok("Customer deleted successfully"));
        }
    }
}