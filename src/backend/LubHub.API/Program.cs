using LubHub.API.Extensions;
using LubHub.API.Middleware;
using LubHub.Application.Extensions;
using LubHub.Infrastructure.Extensions;
using LubHub.Infrastructure.Hubs;
using LubHub.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();

builder.Services.AddVersioning();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<ExceptionHandlingMiddleware>();
builder.Services.AddProblemDetails();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<RaffleHub>("/hubs/raffle");

await app.RunAsync();