using Newtonsoft.Json.Linq;

namespace Medical_Rgistrations.ViewModels
{
    public class ApiResponse
    {
        public string Message { get; set; }
        public string Data { get; set; }
        public bool Success { get; set; }
    }
    public class ApiResponseVM
    {
        public string Message { get; set; }
        public object Data { get; set; }
        public bool Success { get; set; }
    }
}
