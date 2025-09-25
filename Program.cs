using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShovelMonkeys.TheProfessor.Services;

namespace ShovelMonkeys.TheProfessor
{
    public class Program
    {
    public static IServiceProvider? ServiceProviderInstance { get; private set; }

        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<BotService>();
                    services.AddSingleton<CommandHandler>(sp => new CommandHandler(sp.GetRequiredService<IConfiguration>()));

                    // Use local Azurite for Azure Table Storage
                    var tableConn = "UseDevelopmentStorage=true";
                    services.AddSingleton<Data.CampaignRepository>(_ => new Data.CampaignRepository(tableConn));
                    services.AddSingleton<Data.QuestRepository>(_ => new Data.QuestRepository(tableConn));
                })
                .Build();

            ServiceProviderInstance = host.Services;
            await host.Services.GetRequiredService<BotService>().StartAsync();
        }
    }
}
