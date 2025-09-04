using System.Collections.Generic;

namespace Application.Responses
{
    public class BaseResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();
        
        public BaseResponse()
        {
        }
        
        public BaseResponse(string message)
        {
            Message = message;
        }
        
        public BaseResponse(string message, bool success)
        {
            Message = message;
            Success = success;
        }
    }
    
    public class BaseResponse<T> : BaseResponse
    {
        public T? Data { get; set; }
        
        public BaseResponse() : base()
        {
        }
        
        public BaseResponse(T data) : base()
        {
            Data = data;
        }
        
        public BaseResponse(string message) : base(message)
        {
        }
        
        public BaseResponse(string message, bool success) : base(message, success)
        {
        }
        
        public BaseResponse(T data, string message) : base(message)
        {
            Data = data;
        }
    }
}