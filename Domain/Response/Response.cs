using System.Diagnostics.Tracing;
using System.Net;

namespace Domain.Response;

public class Response<T>
{
    public T Data { get; set; }
    public string Errors { get; set; }
    public int StatusCode { get; set; }

    public Response(T data)
    {
        Data = data;
        StatusCode = 200;
    }

    public Response(HttpStatusCode code, string message)
    {
        StatusCode = (int)code;
        Errors = message;
    }
    
    public Response(HttpStatusCode code, string message, T data)
    {
        Data = data;
        StatusCode = (int)code;
        Errors = message;
    }

    public Response(HttpStatusCode code, List<string> messages)
    {
        StatusCode = (int)code;
        // Errors.AddRange(messages);
    }
}