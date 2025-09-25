


using Azure.Data.Tables;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ShovelMonkeys.TheProfessor.Data
{
    public class CampaignRepository
    {
        private readonly TableClient _tableClient;

        public CampaignRepository(string? connectionString = null)
        {
            connectionString ??= "UseDevelopmentStorage=true"; // Azurite/local emulator
            _tableClient = new TableClient(connectionString, "Campaign");
            _tableClient.CreateIfNotExists();
        }

        public string Insert(Campaign campaign)
        {
            if (string.IsNullOrEmpty(campaign.RowKey))
                campaign.RowKey = Guid.NewGuid().ToString();
            _tableClient.AddEntity(campaign);
            return campaign.RowKey;
        }

        public Campaign? GetById(int id)
        {
            var rowKey = id.ToString();
            try
            {
                return _tableClient.GetEntity<Campaign>("Campaign", rowKey).Value;
            }
            catch (Azure.RequestFailedException)
            {
                return null;
            }
        }

        public IEnumerable<Campaign> GetAll()
        {
            return _tableClient.Query<Campaign>(c => c.PartitionKey == "Campaign");
        }

        public bool Update(Campaign campaign)
        {
            try
            {
                _tableClient.UpdateEntity(campaign, campaign.ETag, TableUpdateMode.Replace);
                return true;
            }
            catch (Azure.RequestFailedException)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            var rowKey = id.ToString();
            try
            {
                _tableClient.DeleteEntity("Campaign", rowKey);
                return true;
            }
            catch (Azure.RequestFailedException)
            {
                return false;
            }
        }
    }
}
