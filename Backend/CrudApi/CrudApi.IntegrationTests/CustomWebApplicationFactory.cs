using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace CrudApi.IntegrationTests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing MongoDB registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IMongoDatabase));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add MongoDB with test database
                services.AddSingleton<IMongoDatabase>(sp =>
                {
                    var client = new MongoClient("mongodb://localhost:27017");
                    return client.GetDatabase("CrudApiDb_Test");
                });
            });

            builder.UseEnvironment("Testing");
        }

        public void CleanupDatabase()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            client.DropDatabase("CrudApiDb_Test");
        }
    }
}
