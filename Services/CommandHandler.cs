using System;
using System.Reflection;
using Discord.Interactions;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace ShovelMonkeys.TheProfessor.Services
{
    public class CommandHandler
    {
    private readonly CommandService _commands;
    private InteractionService? _interactions = null;
    private readonly IConfiguration _config;
    private DiscordSocketClient? _client = null;
    private IServiceProvider? _services;

        public CommandHandler(IConfiguration config)
        {
            _commands = new CommandService();
            _config = config;
        }

        public async Task InitializeAsync(DiscordSocketClient client, IServiceProvider services)
        {
            _client = client;
            _services = services;
            _interactions = new InteractionService(_client);
            _client.MessageReceived += HandleCommandAsync;
            _client.InteractionCreated += HandleInteractionAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            // Register slash commands and component handlers to a specific guild
            var modules = await _interactions.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            Console.WriteLine($"[CommandHandler] Registered interaction modules: {string.Join(", ", modules.Select(m => m.Name))}");
            _client.Ready += RegisterGuildCommandsAsync;
        }

        private async Task HandleInteractionAsync(SocketInteraction interaction)
        {
            // Diagnostic logging for interaction debugging
            if (interaction is Discord.WebSocket.SocketMessageComponent component)
            {
                Console.WriteLine($"[DEBUG] ComponentInteraction received: customId={component.Data.CustomId}");
            }
            else
            {
                Console.WriteLine($"[DEBUG] Interaction received: type={interaction.Type}");
            }
            var ctx = new SocketInteractionContext(_client, interaction);
            if (_interactions != null)
                await _interactions.ExecuteCommandAsync(ctx, _services);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            if (!(messageParam is SocketUserMessage message)) return;
            if (message.Author.IsBot) return;

            int argPos = 0;
            if (!(message.HasCharPrefix('!', ref argPos) ||
                  (_client != null && message.HasMentionPrefix(_client.CurrentUser, ref argPos))))
                return;

            var context = new SocketCommandContext(_client, message);
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        private async Task RegisterGuildCommandsAsync()
        {
            if (_config == null) return;
            var guildIdString = _config["Discord:GuildId"];
            if (!ulong.TryParse(guildIdString, out var guildId))
            {
                Console.WriteLine("GuildId is missing or invalid in appsettings.json");
                return;
            }
            if (_interactions != null)
                await _interactions.RegisterCommandsToGuildAsync(guildId, true);
            Console.WriteLine($"Slash commands registered to guild {guildId}");
        }
    }
}
