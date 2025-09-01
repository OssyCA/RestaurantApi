using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Required]
        [StringLength(100)]
        public string Email { get; set; } = null!;
        public DateTime Birthday { get; set; }
        public EmployeeRole EmployeeRole { get; set; }
        public string PasswordHash { get; set; } = null!;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireTime { get; set; }
    }
    public enum EmployeeRole
    {
        Staff = 0,
        Admin = 1,
        Mangement = 2
    }
}



