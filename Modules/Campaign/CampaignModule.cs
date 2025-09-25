using Discord;
using Discord.Interactions;

namespace ShovelMonkeys.TheProfessor.Modules.Campaign
{
    public class CampaignModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly Data.CampaignRepository _campaignRepo;

        public CampaignModule(Data.CampaignRepository campaignRepo)
        {
            _campaignRepo = campaignRepo;
        }

        [ModalInteraction("campaign_add_modal")]
        public async Task HandleCampaignAddModal(CampaignModal modal)
        {
            Console.WriteLine("CampaignModule.HandleCampaignAddModal called");
            // Save campaign to Azure Table Storage
            var campaign = new Data.Campaign
            {
                Name = modal.Name,
                GameSystem = modal.GameSystem,
                Description = modal.Description,
                Owner = Context.User.Username, // or Context.User.Id.ToString() for unique owner
                ImageUrl = string.IsNullOrWhiteSpace(modal.ImageUrl) ? null : modal.ImageUrl.Trim()
            };
            _campaignRepo.Insert(campaign);

            var embedBuilder = new EmbedBuilder()
                .WithTitle(campaign.Name)
                .WithDescription(campaign.Description)
                .WithColor(Color.DarkBlue)
                .AddField("Owner", campaign.Owner)
                .AddField("System", campaign.GameSystem);
            if (!string.IsNullOrWhiteSpace(campaign.ImageUrl))
                embedBuilder.WithImageUrl(campaign.ImageUrl);
            await RespondAsync(embed: embedBuilder.Build(), ephemeral: true);
        }
    }
}
