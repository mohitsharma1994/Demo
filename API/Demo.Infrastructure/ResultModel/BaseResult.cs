using System.Net;

namespace Demo.Infrastructure.ResultModel
{
    public class BaseResult
    {
        public string? Message { get; set; }

        public HttpStatusCode Status { get; set; }
    }
}
