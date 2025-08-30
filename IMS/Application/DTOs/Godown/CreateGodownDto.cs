namespace Application.DTOs.Godown
{
    public class CreateGodownDto
    {
        public required string GodownName { get; set; }
        public required string Location { get; set; }
        public string? GodownManager { get; set; }
    }
}