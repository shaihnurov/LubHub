using LubHub.API.Extensions;
using LubHub.API.Middleware;
using LubHub.Application.Extensions;
using LubHub.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();

builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<ExceptionHandlingMiddleware>();
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.Run();