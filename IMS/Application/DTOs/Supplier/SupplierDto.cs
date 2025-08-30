using Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Supplier
{
    public class SupplierDto : BaseDto
    {
        public required string SupplierName { get; set; }
        public required string ContactPerson { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}