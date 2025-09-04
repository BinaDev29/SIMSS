// Application/DTOs/Customer/CustomerDto.cs
namespace Application.DTOs.Customer
{
    public record CustomerDto
    {
        public int Id { get; init; }
        public string CustomerName { get; init; } = string.Empty;
        public string ContactPerson { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string? Address { get; init; }
        public string? TaxId { get; init; }
        public string? PaymentTerms { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime? LastModifiedDate { get; init; }
    }
}