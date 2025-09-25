using Libba.HubTo.Arcavis.Infrastructure.Persistence.Extensions;
using Libba.HubTo.Arcavis.Application.Extensions;
using Libba.HubTo.Arcavis.WebApi.ActionFilters;
using Libba.HubTo.Arcavis.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEfCoreRegistration(builder.Configuration);
builder.Services.AddArcavisMapper();


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

app.MapGet("/docs", async context =>
{
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(@"
    <!DOCTYPE html>
    <html>
      <head>
        <title>Arcavis API Docs</title>
        <meta charset='UTF-8'/>
        <script src='https://cdn.redoc.ly/redoc/latest/bundles/redoc.standalone.js'></script>
      </head>
      <body>
        <redoc spec-url='/openapi.json'></redoc>
      </body>
    </html>");
});


app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseMiddleware<RequestContextMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
