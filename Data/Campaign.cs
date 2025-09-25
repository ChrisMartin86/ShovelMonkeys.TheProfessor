namespace ShovelMonkeys.TheProfessor.Data
{
    using Azure;
    using Azure.Data.Tables;
    public class Campaign : ITableEntity
    {
        public string PartitionKey { get; set; } = "Campaign";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public string GameSystem { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }

        // For compatibility with old code
        public int Id { get => int.TryParse(RowKey, out var id) ? id : 0; set => RowKey = value.ToString(); }
    }
}
