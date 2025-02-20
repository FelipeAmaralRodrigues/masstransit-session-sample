using Azure.Storage.Blobs;
using MassTransit;
using MassTransitSessionSample;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
    .AddCommandLine(args)
    .AddEnvironmentVariables()
.Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(builder =>
    {
        builder.Sources.Clear();
        builder.AddConfiguration(configuration);
    })
    .UseSerilog((host, log) =>
    {
        log.MinimumLevel.Debug();

        log.MinimumLevel.Override("Microsoft", LogEventLevel.Information);
        log.WriteTo.Console();
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddConsumer<SendDiscountConsumer>();

            x.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("BusConnection"));

                cfg.ReceiveEndpoint("send-discount", ep =>
                {
                    ep.RequiresSession = true;
                    ep.Consumer<SendDiscountConsumer>(context);
                });
            });
        });
        services.AddScoped(x => new BlobServiceClient(configuration.GetConnectionString("StorageAccount")));

    }).Build();

await host.RunAsync();
