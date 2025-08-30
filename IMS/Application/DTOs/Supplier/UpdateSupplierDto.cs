using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Supplier
{
    public class UpdateSupplierDto
    {
        public required int Id { get; set; }
        public string? SupplierName { get; set; }
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}