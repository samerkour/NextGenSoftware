using BuildingBlocks.Monitoring;

namespace NextGen.Api.Extensions.ApplicationBuilderExtensions;

public static partial class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseNextGenMonitoring(this IApplicationBuilder app)
    {
        app.UseMonitoring();

        return app;
    }
}
