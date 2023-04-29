using Persistance.Dbinitialize;

namespace Events.It.Academy.Ge.Api.Infrastructure
{
    public static class SeedDatabase
    {
        public static void Seed(WebApplication app)
        {
            using (IServiceScope scope = app.Services.CreateScope())
                scope.ServiceProvider.GetRequiredService<IDbInitializer>().Initialize();
        }
    }
}

