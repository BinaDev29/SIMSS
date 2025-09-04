// Application/DTOs/Customer/UpdateCustomerDto.cs
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Customer
{
    public record UpdateCustomerDto
    {
        public int Id { get; init; }

        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(100, ErrorMessage = "Customer name cannot exceed 100 characters.")]
        public string CustomerName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Contact person is required.")]
        [StringLength(100, ErrorMessage = "Contact person cannot exceed 100 characters.")]
        public string ContactPerson { get; init; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; init; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; init; } = string.Empty;

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? Address { get; init; }

        public string? TaxId { get; init; }

        public string? PaymentTerms { get; init; }
    }
}