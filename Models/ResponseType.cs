namespace API_CraftyOrnaments.Models
{
    public class ResponseType<T>
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
    

    
    
}
