using DickinsonBros.Cosmos.Extensions;
using DickinsonBros.DataTable.Extensions;
using DickinsonBros.DateTime.Extensions;
using DickinsonBros.DurableRest.Extensions;
using DickinsonBros.Encryption.Certificate.Abstractions;
using DickinsonBros.Encryption.Certificate.Extensions;
using DickinsonBros.Guid.Extensions;
using DickinsonBros.IntegrationTest.Extensions;
using DickinsonBros.Logger.Extensions;
using DickinsonBros.Redactor.Extensions;
using DickinsonBros.SQL.Extensions;
using DickinsonBros.Stopwatch.Extensions;
using DickinsonBros.Telemetry.Extensions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using RollerCoaster.Account.API.Proxy.Extensions;
using RollerCoaster.Account.API.Proxy.Models;
using RollerCoaster.IntegrationTests.API.Infrastructure.AccountDB.Extensions;
using RollerCoaster.IntegrationTests.API.Infrastructure.CoasterDB.Extensions;
using RollerCoaster.IntegrationTests.API.Logic.AccountAPI.Extensions;
using RollerCoaster.IntegrationTests.API.View.Models;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

[assembly: WebJobsStartup(typeof(RollerCoaster.IntegrationTests.API.View.Startup.Startup))]
namespace RollerCoaster.IntegrationTests.API.View.Startup
{
    [ExcludeFromCodeCoverage]
    public class Startup : IWebJobsStartup
    {
        const string _siteRootPath = @"\home\site\wwwroot\";
        const string FUNCTION_ENVIRONMENT_NAME = "FUNCTION_ENVIRONMENT_NAME";
        public void Configure(IWebJobsBuilder builder)
        {
            var configuration = EnrichConfiguration(builder.Services);
            ConfigureServices(builder.Services, configuration);
        }
        private IConfiguration EnrichConfiguration(IServiceCollection serviceCollection)
        {
            var existingConfiguration = serviceCollection.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddConfiguration(existingConfiguration);
            var configTransform = $"appsettings.{System.Environment.GetEnvironmentVariable(FUNCTION_ENVIRONMENT_NAME)}.json";
            var isCICD = !File.Exists(Path.Combine(Directory.GetCurrentDirectory(), configTransform));
            var functionConfigurationRootPath = isCICD ? _siteRootPath : Directory.GetCurrentDirectory();
            var config =
                configurationBuilder
                .SetBasePath(functionConfigurationRootPath)
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile(configTransform, false)
                .Build();
            serviceCollection.Replace(ServiceDescriptor.Singleton(typeof(IConfiguration), config));

            return (IConfiguration)config;
        }
        private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            //#Stack Packages
            services.AddConfigurationEncryptionService();
            ConfigureLogging(services, configuration);
            services.AddDataTableService();
            services.AddGuidService();
            services.AddDateTimeService();
            services.AddStopwatchService();
            services.AddRedactorService();
            services.AddLoggingService();
            services.AddTelemetryService();
            services.AddSQLService();
            services.AddCosmosService();
            services.AddDurableRestService();
            services.AddMemoryCache();
            services.AddIntegrationTestService();

            services.AddAccountProxyService
            (
                new Uri(configuration[$"{nameof(AccountProxyOptions)}:{nameof(AccountProxyOptions.BaseURL)}"]),
                new TimeSpan(0, 0, Convert.ToInt32(configuration[$"{nameof(AccountProxyOptions)}:{nameof(AccountProxyOptions.HttpClientTimeoutInSeconds)}"]))
            );

            //#Local Packages

            //Services
            services.AddAccountDBService();
            services.AddCoasterDBService();

            //Tests
            services.AddAccountAPITests();
        }

        private void ConfigureLogging(IServiceCollection services, IConfiguration configuration)
        {
            var awsOptions = configuration.GetSection("AWSOptions").Get<AWSOptions>();
 
            var provider = services.BuildServiceProvider();
            var configurationEncryptionService = provider.GetRequiredService<IConfigurationEncryptionService>();
            var elasticSearchOptions = configuration.GetSection("ElasticSearchOptions").Get<ElasticSearchOptions>();
            Environment.SetEnvironmentVariable("AWS_REGION", awsOptions.Region);

            services.AddLogging(loggingBuilder =>
            {
                var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configurationEncryptionService.Decrypt(elasticSearchOptions.URL)))
                {
                    IndexFormat = elasticSearchOptions.IndexFormat,
                })
                .CreateLogger();

                loggingBuilder.AddSerilog
                (
                    logger,
                    dispose: true
                );

                loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
            });
        }
    }
}

