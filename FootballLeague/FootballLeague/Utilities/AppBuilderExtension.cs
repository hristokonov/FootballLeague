using FootballLeague.Utilities.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace FootballLeague.Utilities
{
    public static class AppBuilderExtension
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
