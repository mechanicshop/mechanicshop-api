using MechanicShop.Api.Endpoints;
using MechanicShop.Infrastructure.Data;
using MechanicShop.Infrastructure.RealTime;
using MechanicShop.Infrastructure.Settings;

using Microsoft.Extensions.Options;

using Scalar.AspNetCore;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation(builder.Configuration)
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "MechanicShop API V1");

        options.EnableDeepLinking();
        options.DisplayRequestDuration();
        options.EnableFilter();
    });

    app.MapScalarApiReference();

    await app.InitialiseDatabaseAsync();
}
else
{
    app.UseHsts();
}

var appSettings = app.Services.GetRequiredService<IOptions<AppSettings>>().Value;
app.UseCoreMiddlewares(appSettings.Cors.PolicyName);

app.MapPrometheusScrapingEndpoint();

app.MapAllEndpoints();

app.MapHub<WorkOrderHub>("/hubs/workorders");

app.Run();
