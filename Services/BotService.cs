using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace ShovelMonkeys.TheProfessor.Services
{
    public class BotService
    {
        private readonly DiscordSocketClient _client;
        private readonly IConfiguration _config;
    private readonly CommandHandler _commandHandler;

    public BotService(IConfiguration config, CommandHandler commandHandler)
        {
            _config = config;
            _commandHandler = commandHandler;
            _client = new DiscordSocketClient(new DiscordSocketConfig {
                GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.MessageContent
            });
        }

        // (Removed duplicate StartAsync, see below for the version with logging)
            public async Task StartAsync()
            {
                try
                {
                    Console.WriteLine("[BotService] Starting Discord bot...");
                    _client.Log += LogAsync;
                    _client.Ready += ReadyAsync;
                    Console.WriteLine("[BotService] Initializing command handler...");
                    await _commandHandler.InitializeAsync(_client, Program.ServiceProviderInstance!);

                    var token = _config["Discord:Token"];
                    if (string.IsNullOrWhiteSpace(token))
                    {
                        Console.WriteLine("[BotService] Bot token is missing in appsettings.json");
                        return;
                    }

                    Console.WriteLine("[BotService] Logging in to Discord...");
                    await _client.LoginAsync(TokenType.Bot, token);
                    Console.WriteLine("[BotService] Starting Discord client...");
                    await _client.StartAsync();

                    Console.WriteLine("[BotService] Bot is running. Waiting for events...");
                    await Task.Delay(-1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[BotService ERROR] {ex.Message}\n{ex.StackTrace}");
                    throw;
                }
            }

        private Task LogAsync(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            Console.WriteLine($"Connected as -> {_client.CurrentUser}");
            return Task.CompletedTask;
        }
    }
}
