using Discord;

namespace ShovelMonkeys.TheProfessor.Modules.Campaign
{
    public static class CampaignEmbedHelper
    {
        public static Embed BuildCampaignEmbed(string name, string owner, string gameSystem, string? description = null)
        {
            var builder = new EmbedBuilder()
                .WithTitle($"Campaign: {name}")
                .AddField("Owner", owner, true)
                .AddField("Game System", gameSystem, true)
                .WithColor(Color.DarkBlue);
            if (!string.IsNullOrWhiteSpace(description))
                builder.AddField("Description", description);
            return builder.Build();
        }
    }
}
