// Application/Responses/BaseCommandResponse.cs
using System.Collections.Generic;

namespace Application.Responses
{
    public class BaseCommandResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public int Id { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}