using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebConfig.IoC;
using WebMarkupMin.AspNetCore8;

namespace WebConfig.Middlewares;

public static class MiddlewaresExtentions
{
    public static void UseAllMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
            });
        }
        else
        {
            app.UseApplicationExceptionHandler();
            app.UseHsts();
        }
        //app.UseNotFoundHandler();

        //app.UseHttpsRedirection();
        //app.MapControllers();

        //app.UseStaticFiles();
        //app.UseWebMarkupMin();

        //app.UseRouting();


        //app.Run();
        app.UseHttpsRedirection();

        app.MapControllers();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors(ServiceInjectionExtentions.AllowedOrigins);
        app.Run();
    }
}