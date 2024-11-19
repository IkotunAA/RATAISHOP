namespace RATAISHOP.Models
{
    public class BaseResponse<T>
    {
        public bool Status { get; set; } 
        public string? Message { get; set; } = default!;
        public T? Data { get; set; } = default;
        //public BaseResponse(bool success, string message)
        //{
        //    Status = success;
        //    Message = message;
        //}

        //public BaseResponse(bool success, string message, T data)
        //{
        //    Status = success;
        //    Message = message;
        //    Data = data;
        //}

    }

}
