using Application.Contracts.Interfaces.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Reflection;
using IInjectableService = Application.Contracts.Interfaces.Common.IInjectableService;

namespace WebConfig.IoC;

public static class DependencyContainer
{
    /// <summary>
    /// This will register all services that are inheriting from IInjectableService.
    /// </summary>
    /// <param name="services"></param>
    public static void RegisterContractServices(this IServiceCollection services)
	{
		// Get the assembly where your services are defined
		Assembly assembly = Assembly.GetAssembly(typeof(IInjectableService));

		// Find all types in the assembly that implement the IService interface
		var serviceTypes = assembly.GetTypes()
			.Where(type => type.IsClass && !type.IsAbstract && type.GetInterfaces().Any(inter => inter == typeof(IInjectableService)))
			.ToList();

		// Register each service type
		foreach (var serviceType in serviceTypes)
		{
			var name = $"I{serviceType.Name}";
			var interfaceType = serviceType.GetInterfaces().FirstOrDefault(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

			var attribute = serviceType.GetCustomAttribute<ServiceLifetimeAttribute>();
			var lifetime = attribute?.Lifetime ?? ServiceLifetime.Scoped;

			switch (lifetime)
			{
				case ServiceLifetime.Transient:
					if (interfaceType is null) services.AddTransient(serviceType);
					else services.AddTransient(interfaceType, serviceType);
					break;
				case ServiceLifetime.Singleton:
					if (interfaceType is null) services.AddSingleton(serviceType);
					else services.AddSingleton(interfaceType, serviceType);
					break;
				case ServiceLifetime.Scoped:
				default:
					if (interfaceType is null) services.AddScoped(serviceType);
					else services.AddScoped(interfaceType, serviceType);
					break;
			}
		}
	}
}