namespace Application.Responses
{
    public class ApiResponse<T> : BaseCommandResponse
    {
        public T? Data { get; set; }
    }
}
