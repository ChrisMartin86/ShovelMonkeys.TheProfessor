using Discord;
using Discord.Interactions;

namespace ShovelMonkeys.TheProfessor.Modules.Quest
{
    public class QuestModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly Data.QuestRepository _questRepo;
        private readonly Data.CampaignRepository _campaignRepo;
        public QuestModule(Data.QuestRepository questRepo, Data.CampaignRepository campaignRepo)
        {
            _questRepo = questRepo;
            _campaignRepo = campaignRepo;
        }


        [ComponentInteraction("quest_add_campaign_select")]
        public async Task HandleCampaignSelect(string[] selected)
        {
            if (selected.Length == 0)
            {
                await RespondAsync("No campaign selected.", ephemeral: true);
                return;
            }
            var campaignRowKey = selected[0];
            // Encode campaignRowKey in the modal customId
            var modalCustomId = $"quest_add_modal:{campaignRowKey}";
            await Context.Interaction.RespondWithModalAsync<QuestModal>(modalCustomId);
        }

        [ModalInteraction("quest_add_modal:{campaignRowKey}")]
        public async Task HandleQuestAddModal(string campaignRowKey, QuestModal modal)
        {
            Console.WriteLine($"QuestModule.HandleQuestAddModal called for campaignRowKey={campaignRowKey}");
            var campaign = _campaignRepo.GetAll().FirstOrDefault(c => c.RowKey == campaignRowKey);
            if (campaign == null)
            {
                await RespondAsync("Campaign not found.", ephemeral: true);
                return;
            }
            var quest = new Data.Quest {
                CampaignRowKey = campaignRowKey,
                Name = modal.Name,
                Description = modal.Description,
                ImageUrl = string.IsNullOrWhiteSpace(modal.ImageUrl) ? null : modal.ImageUrl.Trim()
            };
            var id = _questRepo.Insert(quest);
            var stored = _questRepo.GetById(int.TryParse(id, out var intId) ? intId : 0);
            var embedBuilder = new Discord.EmbedBuilder()
                .WithTitle(stored?.Name ?? modal.Name)
                .WithDescription(stored?.Description ?? modal.Description)
                .WithColor(Discord.Color.DarkPurple)
                .AddField("Campaign", campaign.Name);
            if (!string.IsNullOrWhiteSpace(quest.ImageUrl))
                embedBuilder.WithImageUrl(quest.ImageUrl);
            await RespondAsync(embed: embedBuilder.Build(), ephemeral: true);
        }
    }
}
