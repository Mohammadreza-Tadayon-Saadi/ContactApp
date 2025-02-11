using Presentation.Controllers;
using WebConfig.IoC;
using WebConfig.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAllServices(builder.Configuration, builder.Host, typeof(ContactsController).Assembly);

var app = builder.Build();
app.UseAllMiddlewares();