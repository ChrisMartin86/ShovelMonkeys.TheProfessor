


using Azure.Data.Tables;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ShovelMonkeys.TheProfessor.Data
{
    public class QuestRepository
    {
        private readonly TableClient _tableClient;

        public QuestRepository(string? connectionString = null)
        {
            connectionString ??= "UseDevelopmentStorage=true"; // Azurite/local emulator
            _tableClient = new TableClient(connectionString, "Quest");
            _tableClient.CreateIfNotExists();
        }

        public string Insert(Quest quest)
        {
            if (string.IsNullOrEmpty(quest.RowKey))
                quest.RowKey = Guid.NewGuid().ToString();
            _tableClient.AddEntity(quest);
            return quest.RowKey;
        }

        public Quest? GetById(int id)
        {
            var rowKey = id.ToString();
            try
            {
                return _tableClient.GetEntity<Quest>("Quest", rowKey).Value;
            }
            catch (Azure.RequestFailedException)
            {
                return null;
            }
        }


        public IEnumerable<Quest> GetByCampaignRowKey(string campaignRowKey)
        {
            return _tableClient.Query<Quest>(q => q.PartitionKey == "Quest" && q.CampaignRowKey == campaignRowKey);
        }

        public IEnumerable<Quest> GetAll()
        {
            return _tableClient.Query<Quest>(q => q.PartitionKey == "Quest");
        }

        public bool Update(Quest quest)
        {
            try
            {
                _tableClient.UpdateEntity(quest, quest.ETag, TableUpdateMode.Replace);
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
                _tableClient.DeleteEntity("Quest", rowKey);
                return true;
            }
            catch (Azure.RequestFailedException)
            {
                return false;
            }
        }
    }
}
