using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;

namespace ShovelMonkeys.TheProfessor.Modules.Quest
{
    [Group("quest", "Manage quests for a campaign.")]
    public class QuestGroup : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly Data.QuestRepository _questRepo;
        private readonly Data.CampaignRepository _campaignRepo;
        public QuestGroup(Data.QuestRepository questRepo, Data.CampaignRepository campaignRepo)
        {
            _questRepo = questRepo;
            _campaignRepo = campaignRepo;
        }

        [SlashCommand("add", "Add a quest to a campaign.")]
        public async Task Add()
        {
            var campaigns = _campaignRepo.GetAll().ToList();
            if (!campaigns.Any())
            {
                await RespondAsync("You must register a Campaign before you can add a quest", ephemeral: true);
                return;
            }
            var selectMenu = new SelectMenuBuilder()
                .WithCustomId("quest_add_campaign_select")
                .WithPlaceholder("Select a campaign")
                .WithMinValues(1)
                .WithMaxValues(1);
            foreach (var c in campaigns)
                selectMenu.AddOption(c.Name, c.RowKey);
            var builder = new ComponentBuilder().WithSelectMenu(selectMenu);
            await RespondAsync("Choose a campaign for the new quest:", components: builder.Build(), ephemeral: true);
        }


        [SlashCommand("update", "Update a quest in a campaign.")]
        public async Task Update(
            [Summary("campaign", "Campaign name")] string campaign,
            [Summary("quest", "Quest name")] string quest,
            [Summary("description", "Quest description")] string? description = null)
        {
            await UpdateQuest(campaign, quest, description);
        }

        [SlashCommand("delete", "Delete a quest from a campaign.")]
        public async Task Delete(
            [Summary("campaign", "Campaign name")] string campaign,
            [Summary("quest", "Quest name")] string quest)
        {
            await DeleteQuest(campaign, quest);
        }

        private async Task AddQuest(string campaign, string quest, string? description)
        {
            // TODO: Implement quest storage
            var embed = QuestEmbedHelper.BuildQuestEmbed(campaign, quest, description);
            await RespondAsync(embed: embed, ephemeral: true);
        }

        private Task UpdateQuest(string campaign, string quest, string? description)
        {
            // TODO: Implement quest update
            throw new System.NotImplementedException("Quest update not yet implemented.");
        }

        private Task DeleteQuest(string campaign, string quest)
        {
            // TODO: Implement quest deletion
            throw new System.NotImplementedException("Quest deletion not yet implemented.");
        }

        [SlashCommand("list", "List all quests for a campaign.")]
        public async Task List(
            [Summary("campaign", "Campaign name or ID")] string campaign)
        {
            Data.Campaign? camp = _campaignRepo.GetAll().FirstOrDefault(c => c.RowKey == campaign || c.Name.Equals(campaign, StringComparison.OrdinalIgnoreCase));
            if (camp == null)
            {
                await RespondAsync($"No campaign found for '{campaign}'.", ephemeral: true);
                return;
            }
            var quests = _questRepo.GetByCampaignRowKey(camp.RowKey).ToList();
            if (!quests.Any())
            {
                await RespondAsync($"No quests found for campaign '{camp.Name}'.", ephemeral: true);
                return;
            }
            var embed = new EmbedBuilder()
                .WithTitle($"Quests for {camp.Name}")
                .WithColor(Color.DarkPurple);
            foreach (var q in quests)
                embed.AddField($"{q.Name} (ID: {q.Id})", q.Description);
            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }

        [SlashCommand("get", "Get a quest by name or ID.")]
        public async Task Get(
            [Summary("quest", "Quest name or ID")] string quest)
        {
            Data.Quest? q = null;
            if (int.TryParse(quest, out var id))
                q = _questRepo.GetById(id);
            if (q == null)
                q = _questRepo.GetAll().FirstOrDefault(x => x.Name.Equals(quest, StringComparison.OrdinalIgnoreCase));
            if (q == null)
            {
                await RespondAsync($"No quest found for '{quest}'.", ephemeral: true);
                return;
            }
            var campaign = _campaignRepo.GetById(q.CampaignId);
            var embed = QuestEmbedHelper.BuildQuestEmbed(campaign?.Name ?? "Unknown", q.Name, q.Description);
            await RespondAsync(embed: embed, ephemeral: true);
        }
    }
}
