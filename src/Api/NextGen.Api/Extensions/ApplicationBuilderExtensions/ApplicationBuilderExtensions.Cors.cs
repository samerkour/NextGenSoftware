namespace NextGen.Api.Extensions.ApplicationBuilderExtensions;

public static partial class ApplicationBuilderExtensions
{
    /// <summary>
    ///     Register CORS.
    /// </summary>
    public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
    {
        app.UseCors(policy =>
        {
            policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });

        return app;
    }
}
