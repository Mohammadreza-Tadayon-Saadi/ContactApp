using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using FluentValidation.AspNetCore;
using System.Reflection;
using Application.CustomMappings.Configurations;
using WebConfig.Configurations;
using Application.Contracts.Services.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebMarkupMin.AspNetCore8;
using Asp.Versioning;
using Infrastructure.Context;
using Infrastructure.Settings;
using Core.Utilities;

namespace WebConfig.IoC;

public static class ServiceInjectionExtentions
{
	private static readonly Assembly ApplicationAssembly = typeof(BaseServices<>).Assembly;
	public static Assembly _presentationAssembly;
	public static string AllowedOrigins = nameof(AllowedOrigins);

    public static void AddAllServices(this IServiceCollection services, IConfiguration configuration, ConfigureHostBuilder hostBuilder, Assembly presentationAssembly)
	{
		_presentationAssembly = presentationAssembly;

		services.AddCORS();

		services.AddControllers(options =>
		{
			//options.Filters.Add(new AuthorizeFilter()); // Every Action Have Authorize Attribute
		}).AddFluentValidationService();

		services.AddContext(configuration);

		services.AddMarkupMin();

		services.AddWebAuthentication();
		services.AddAuthorization();
		services.AddCustomInjections(configuration);

        services.AddCustomApiVersioning();

		services.AddMediatRService();

		hostBuilder.AddSerilog();

        services.AddSwaggerGen();

        AutoMapperConfiguration.InitializeAutoMapper();

		services.AddCustomConfigurations();

		services.RegisterContractServices();
	}

	private static void AddCORS(this IServiceCollection services)
	{
        services.AddCors(options =>
        {
            options.AddPolicy(name: AllowedOrigins,
                              policy =>
                              {
                                  policy.WithOrigins("http://localhost:3000",
                                                     "https://localhost:3000")
                                                    .AllowAnyHeader()
                                                    .AllowAnyMethod();
                              });
        });
    }

	private static void AddContext(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<DataBaseContext>(options =>
		{
			options.UseSqlServer(configuration.GetConnectionString(ConnectionStrings.ContactsConnection), sqlOptions =>
			{
				var timeoutSection = configuration.GetSection(ConnectionStrings.TimeoutConnection);
				var timeoutValue = (timeoutSection is not null && timeoutSection.Value is not null) ? timeoutSection.Value.ToInt() : 30;

				sqlOptions.CommandTimeout(timeoutValue);
			});
			options.EnableSensitiveDataLogging();
		});
	}

	private static void AddSerilog(this ConfigureHostBuilder hostBuilder)
	{
		hostBuilder.UseSerilog((hostingContext, loggerConfiguration) =>
		{
			loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
		});
	}

	private static void AddFluentValidationService(this IMvcBuilder mvcBuilder)
	{
		mvcBuilder.AddFluentValidation(options =>
		{
			options.AutomaticValidationEnabled = true;
			options.RegisterValidatorsFromAssembly(ApplicationAssembly);
			options.DisableDataAnnotationsValidation = true;
			options.LocalizationEnabled = true;
		});
	}

    private static void AddMediatRService(this IServiceCollection services)
	{
		services.AddMediatR(option =>
		{
			option.RegisterServicesFromAssembly(ApplicationAssembly);
		});
	}

	private static void AddMarkupMin(this IServiceCollection services)
	{
		services.AddWebMarkupMin(options =>
		{
			options.AllowMinificationInDevelopmentEnvironment = true;
			options.AllowCompressionInDevelopmentEnvironment = true;
		}).AddHtmlMinification()
		  .AddXmlMinification()
		  .AddHttpCompression();
	}

	private static void AddWebAuthentication(this IServiceCollection services)
	{
		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
		}).AddCookie(options =>
		{
			options.Cookie.Name = "BazdidCarAuthCookie";
			options.LoginPath = "/AdminPanel/LogIn";
			options.LogoutPath = "/AdminPanel/LogOut";
			options.ExpireTimeSpan = TimeSpan.FromDays(1);
		});
	}

	private static void AddCustomApiVersioning(this IServiceCollection services)
	{
		services.AddApiVersioning(options =>
		{
			//url segment => {version}
			options.DefaultApiVersion = new ApiVersion(1, 0); //v1.0 == v1
			options.AssumeDefaultVersionWhenUnspecified = true; //default => false;
			options.ReportApiVersions = true;
			//options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
			// api/posts?api-version=1

			//api/v1/posts
			options.ApiVersionReader = new UrlSegmentApiVersionReader();

			//options.ApiVersionReader = new HeaderApiVersionReader(new[] { "Api-Version" });
			// header => Api-Version : 1

			//options.ApiVersionReader = new MediaTypeApiVersionReader()

			//options.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version"), new UrlSegmentApiVersionReader())
			// combine of [querystring] & [urlsegment]
		}).AddApiExplorer(options =>
		{
			options.GroupNameFormat = "'v'VVV";
			options.SubstituteApiVersionInUrl = true;
		});
	}

	private static void AddCustomInjections(this IServiceCollection services, IConfiguration configuration)
	{
    }
}