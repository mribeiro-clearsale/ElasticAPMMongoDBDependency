using Elastic.Apm.MongoDb;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace ElasticAPMMongoDBDependency;

public static class MongoDbExtensions
{
    private const string ConventionPackName = "pack";

    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var pack = new ConventionPack
        {
            new CamelCaseElementNameConvention()
        };
        ConventionRegistry.Register(ConventionPackName, pack, _ => true);

        services.AddSingleton(provider =>
        {            

            var mongoSettings = MongoClientSettings.FromConnectionString("connectionString");
            mongoSettings.ServerApi = new ServerApi(ServerApiVersion.V1);

            mongoSettings.ClusterConfigurator = builder => builder.Subscribe(new MongoDbEventSubscriber());            

            return new MongoClient(mongoSettings);
        });

        return services;
    }
}
