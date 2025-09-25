using Discord;

namespace ShovelMonkeys.TheProfessor.Modules.Quest
{
    public static class QuestEmbedHelper
    {
        public static Embed BuildQuestEmbed(string campaign, string quest, string? description)
        {
            var builder = new EmbedBuilder()
                .WithTitle($"Quest: {quest}")
                .AddField("Campaign", campaign, true)
                .AddField("Name", quest, true)
                .WithColor(Color.DarkPurple);
            if (!string.IsNullOrWhiteSpace(description))
                builder.AddField("Description", description);
            return builder.Build();
        }
    }
}
