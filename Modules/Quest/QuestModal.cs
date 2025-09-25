using Discord.Interactions;

namespace ShovelMonkeys.TheProfessor.Modules.Quest
{
    public class QuestModal : IModal
    {
        public string Title { get; set; } = "Quest";

        [InputLabel("Quest Name")]
        [ModalTextInput("name", placeholder: "Enter quest name", maxLength: 100)]
        public string Name { get; set; } = string.Empty;

        [InputLabel("Description")]
        [ModalTextInput("description", placeholder: "Describe the quest", maxLength: 1000)]
        public string Description { get; set; } = string.Empty;

    [InputLabel("Image URL (optional)")]
    [ModalTextInput("image_url", placeholder: "Paste an image URL (optional)", maxLength: 300)]
    public string? ImageUrl { get; set; }
    }
}
