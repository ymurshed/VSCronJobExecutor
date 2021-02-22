using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using VSCronJobExecutor.ApiService.Interfaces;
using VSCronJobExecutor.ApiService.Services;
using VSCronJobExecutor.Common;
using VSCronJobExecutor.Common.Models.OptionModels;

namespace VSCronJobExecutor.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConfigureServices();
                LogHelper.Print("---------->>> Started the VSCronJobExecutor successfully.");

                Task.Run(async () =>
                {
                    await Task.Delay(3 * 1000);
                    var executor = new Executor();
                    await executor.Execute();
                }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                LogHelper.Print(ex, "---------->>> The VSCronJobExecutor is exiting due to unhandled exception...!", true);
            }

            LogHelper.Print("---------->>> Completed the VSCronJobExecutor successfully.");
            Log.CloseAndFlush();
            Environment.Exit(0);
        }

        private static void ConfigureServices()
        {
            #region Build configuration
            var appBasePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            Console.WriteLine("App Path: " + appBasePath);

            ServiceConfigurationInstance.Configuration = new ConfigurationBuilder()
                .SetBasePath(appBasePath)
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .Build();
            #endregion

            #region Config logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .ReadFrom.Configuration(ServiceConfigurationInstance.Configuration)
                .CreateLogger();
            #endregion

            #region Build service provider
            var services = new ServiceCollection().AddOptions();
            AddGroupedOptions(services);
            AddAllServices(services);
            services.AddLogging(o => { o.AddSerilog(); });
            ServiceConfigurationInstance.ServiceProvider = services.BuildServiceProvider();
            #endregion
        }

        private static void AddAllServices(IServiceCollection services)
        {
            #region Api Services
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IJobService, JobService>();
            #endregion
        }

        private static void AddGroupedOptions(IServiceCollection services)
        {
            services.Configure<VSCronOptions>(ServiceConfigurationInstance.Configuration.GetSection(nameof(VSCronOptions)));
        }
    }
}
