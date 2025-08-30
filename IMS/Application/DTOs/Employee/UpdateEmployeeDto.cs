using System;

namespace Application.DTOs.Employee
{
    public class UpdateEmployeeDto
    {
        public required int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? JoinDate { get; set; }
        public string? Email { get; set; }
        public string? JobTitle { get; set; }
        public string? Department { get; set; }
        public bool IsActive { get; set; }
        public decimal? Salary { get; set; }
    }
}