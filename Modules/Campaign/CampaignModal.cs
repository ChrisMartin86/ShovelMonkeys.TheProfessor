using Discord.Interactions;

namespace ShovelMonkeys.TheProfessor.Modules.Campaign
{
    public class CampaignModal : IModal
    {
        public string Title { get; set; } = "Campaign";

        [InputLabel("Campaign Name")]
        [ModalTextInput("name", placeholder: "Enter campaign name", maxLength: 100)]
        public string Name { get; set; } = string.Empty;

        [InputLabel("Game System")]
        [ModalTextInput("system", placeholder: "Enter game system", maxLength: 100)]
        public string GameSystem { get; set; } = string.Empty;

        [InputLabel("Description")]
        [ModalTextInput("description", placeholder: "Describe the campaign", maxLength: 1000)]
        public string Description { get; set; } = string.Empty;

    }
}
