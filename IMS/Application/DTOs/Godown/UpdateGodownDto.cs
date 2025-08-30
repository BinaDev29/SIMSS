namespace Application.DTOs.Godown
{
    public class UpdateGodownDto
    {
        public required int Id { get; set; }
        public required string GodownName { get; set; }
        public required string Location { get; set; }
        public string? GodownManager { get; set; }
    }
}