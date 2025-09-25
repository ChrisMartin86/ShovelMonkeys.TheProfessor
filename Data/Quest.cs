namespace ShovelMonkeys.TheProfessor.Data
{
    using Azure;
    using Azure.Data.Tables;
    public class Quest : ITableEntity
    {
        public string PartitionKey { get; set; } = "Quest";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Link to campaign by RowKey (string)
        public string CampaignRowKey { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }

        // For compatibility with old code
        public int Id { get => int.TryParse(RowKey, out var id) ? id : 0; set => RowKey = value.ToString(); }
        public int CampaignId { get => int.TryParse(CampaignRowKey, out var id) ? id : 0; set => CampaignRowKey = value.ToString(); }
    }
}
