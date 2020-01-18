// ASP.NET Core middleware migrated from a handler

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace dotnet.Middleware
{
    public class ImageHandlerMiddleware
    {

        // Must have constructor with this signature, otherwise exception at run time
        public ImageHandlerMiddleware(RequestDelegate next)
        {
            // This is an HTTP Handler, so no need to store next
        }

        public async Task Invoke(HttpContext context)
        {
            var branchVer = context.Request.Query["branch"];
            context.Response.ContentType = GetContentType();
            await context.Response.WriteAsync($"Branch used = {branchVer}");
        }

        private string GetContentType()
        {
            return "text/plain";
        }
    }

    public static class ImageHandlerExtensions
    {
        public static IApplicationBuilder UseImageHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageHandlerMiddleware>();
        }
    }
}
