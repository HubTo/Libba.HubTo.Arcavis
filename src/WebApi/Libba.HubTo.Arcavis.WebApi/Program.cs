using Libba.HubTo.Arcavis.Infrastructure.Persistence.Extensions;
using Libba.HubTo.Arcavis.Infrastructure.Redis.Extensions;
using Libba.HubTo.Arcavis.Application.Extensions;
using Libba.HubTo.Arcavis.WebApi.ActionFilters;
using Libba.HubTo.Arcavis.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEfCoreRegistration(builder.Configuration);
builder.Services.AddApplicationRegistration();
builder.Services.AddRedisRegistration(builder.Configuration);


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelActionFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseMiddleware<RequestContextMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
