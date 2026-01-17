using CrudApi.Application.Interfaces;
using CrudApi.Infrastructure.Repositories;
using MongoDB.Driver;

namespace CrudApi.Api.Infrastructure.ServiceRegistrations
{
    public static class RegisterMongo
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, string connectionString, string databaseName)
        {
            var mongoClient = new MongoClient(connectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseName);

            services.AddSingleton(mongoDatabase);
            services.AddScoped<IUserRepository, MongoUserRepository>();

            return services;
        }
    }
}
