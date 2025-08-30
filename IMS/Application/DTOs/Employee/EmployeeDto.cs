using Application.DTOs.Common;
using Application.DTOs.User;
using System;

namespace Application.DTOs.Employee
{
    public class EmployeeDto : BaseDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? JoinDate { get; set; }
        public string? Email { get; set; }
        public string? JobTitle { get; set; }
        public string? Department { get; set; }
        public bool IsActive { get; set; }
        public decimal? Salary { get; set; }

        public required int UserId { get; set; }
        public UserDto? User { get; set; }
    }
}