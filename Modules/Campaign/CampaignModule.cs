
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.IO;
using Discord;
using Discord.Interactions;

namespace ShovelMonkeys.TheProfessor.Modules.Campaign
{

    public class CampaignModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly Data.CampaignRepository _campaignRepo;
        private readonly IConfiguration _config;

        public CampaignModule(Data.CampaignRepository campaignRepo, IConfiguration config)
        {
            _campaignRepo = campaignRepo;
            _config = config;
        }


        [ModalInteraction("campaign_add_modal")]
        public async Task HandleCampaignAddModal(CampaignModal modal)
        {
            Console.WriteLine("CampaignModule.HandleCampaignAddModal called");
            var campaign = new Data.Campaign
            {
                Name = modal.Name,
                GameSystem = modal.GameSystem,
                Description = modal.Description,
                Owner = Context.User.Username
            };
            var rowKey = _campaignRepo.Insert(campaign);
            campaign.RowKey = rowKey;

            // Prompt user with two buttons: Upload Image or Skip
            var builder = new ComponentBuilder()
                .WithButton("Upload Image", customId: $"campaign_upload_image:{rowKey}", ButtonStyle.Primary)
                .WithButton("Skip", customId: $"campaign_skip_image:{rowKey}", ButtonStyle.Secondary);
            await RespondAsync("Campaign saved! Would you like to upload an image?", components: builder.Build(), ephemeral: true);
        }
    }
}
