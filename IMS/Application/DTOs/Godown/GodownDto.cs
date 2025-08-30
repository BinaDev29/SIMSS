using Application.DTOs.Common;

namespace Application.DTOs.Godown
{
    public class GodownDto : BaseDto
    {
        public required string GodownName { get; set; }
        public required string Location { get; set; }
        public string? GodownManager { get; set; }
    }
}