using Discord;
using Discord.Interactions;

namespace ShovelMonkeys.TheProfessor.Modules.Campaign
{
    [Group("campaign", "Manage campaigns.")]
    public class CampaignGroup : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly Data.CampaignRepository _campaignRepo;
        public CampaignGroup(Data.CampaignRepository campaignRepo)
        {
            _campaignRepo = campaignRepo;
        }
        [SlashCommand("add", "Add a campaign.")]
        public async Task Add()
        {
            Console.WriteLine("CampaignGroup.Add called");
            await Context.Interaction.RespondWithModalAsync<CampaignModal>("campaign_add_modal");
        }

        [SlashCommand("list", "List all campaigns.")]
        public async Task List()
        {
            var campaigns = _campaignRepo.GetAll().ToList();
            if (!campaigns.Any())
            {
                await RespondAsync("No campaigns found.", ephemeral: true);
                return;
            }
            var embed = new EmbedBuilder()
                .WithTitle("Campaigns")
                .WithColor(Color.DarkBlue);
            foreach (var c in campaigns)
                embed.AddField($"{c.Name} (ID: {c.Id})", $"Owner: {c.Owner}\nSystem: {c.GameSystem}\n{c.Description}");
            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }

        [SlashCommand("get", "Get a campaign by name or ID.")]
        public async Task Get(
            [Summary("name_or_id", "Campaign name or ID")] string nameOrId)
        {
            Data.Campaign? campaign = null;
            if (int.TryParse(nameOrId, out var id))
                campaign = _campaignRepo.GetById(id);
            if (campaign == null)
                campaign = _campaignRepo.GetAll().FirstOrDefault(c => c.Name.Equals(nameOrId, StringComparison.OrdinalIgnoreCase));
            if (campaign == null)
            {
                await RespondAsync($"No campaign found for '{nameOrId}'.", ephemeral: true);
                return;
            }
            var embed = CampaignEmbedHelper.BuildCampaignEmbed(campaign.Name, campaign.Owner, campaign.GameSystem, campaign.Description);
            await RespondAsync(embed: embed, ephemeral: true);
        }


        [SlashCommand("update", "Update a campaign.")]      
        public async Task Update(
            [Summary("name", "Campaign name")] string name,
            [Summary("owner", "Owner")] string owner,
            [Summary("system", "Game System")] string? gameSystem = null,
            [Summary("description", "Description")] string? description = null)
        {
            Console.WriteLine("CampaignGroup.Update called");
            await UpdateCampaign(name, owner, gameSystem, description);
        }

        [SlashCommand("delete", "Delete a campaign.")]
        public async Task Delete(
            [Summary("name", "Campaign name")] string name)
        {
            Console.WriteLine("CampaignGroup.Delete called");
            await DeleteCampaign(name);
        }

        private Task UpdateCampaign(string name, string owner, string? gameSystem, string? description)
        {
            Console.WriteLine($"UpdateCampaign called: name={name}, owner={owner}, system={gameSystem}, description={description}");
            // TODO: Implement campaign update
            throw new System.NotImplementedException("Campaign update not yet implemented.");
        }

        private Task DeleteCampaign(string name)
        {
            Console.WriteLine($"DeleteCampaign called: name={name}");
            // TODO: Implement campaign deletion
            throw new System.NotImplementedException("Campaign deletion not yet implemented.");
        }
    }
}
