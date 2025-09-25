using Discord.Commands;
using Discord.Interactions;

namespace ShovelMonkeys.TheProfessor.Modules
{
    // Text command module

    public class BasicModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("Pong!");
        }
    }

    // Slash command module
    public class BasicSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ping", "Replies with Pong!")]
        public async Task PingSlashAsync()
        {
            await RespondAsync("Pong!");
        }
    }
}
