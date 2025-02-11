using Application.Contracts.Services.Common;
using System.Reflection;

namespace Application.CustomMappings.Configurations;

public static class AutoMapperConfiguration
{
    public static IMapper Mapper { get; }
    static AutoMapperConfiguration()
    {
        var configureProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddCustomMappingProfile();
        });

        configureProvider.AssertConfigurationIsValid();
        configureProvider.CompileMappings();

        Mapper = configureProvider.CreateMapper();
    }

    // This Method Is That For Going To Static Constructor
    public static void InitializeAutoMapper() {}

    public static void AddCustomMappingProfile(this IMapperConfigurationExpression config)
    {
        config.AddCustomMappingProfile(typeof(BaseServices<>).Assembly);
    }

    public static void AddCustomMappingProfile(this IMapperConfigurationExpression config, params Assembly[] assemblies)
    {
        /*
         Every Assembly Have ExportedTypes That Returns Every Class Which Can Export To OutSide.
         It Means That The Class Must Be Public 
        */
        var allTypes = assemblies.SelectMany(assembly => assembly.ExportedTypes); 

        var listOfCustomMapping = allTypes
            .Where(type => type.IsClass && !type.IsAbstract && 
                    type.GetInterfaces().Contains(typeof(IHaveCustomMapping)))
            .Select(type => (IHaveCustomMapping)Activator.CreateInstance(type));

        var profile = new CustomMappingProfile(listOfCustomMapping);
        config.AddProfile(profile);
    }
}