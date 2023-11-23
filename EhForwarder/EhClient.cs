using Azure;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Text;

namespace EhForwarder
{
    public class EhClient
    {
        private readonly EventHubProducerClient _eventHubProducerClient;

        public EhClient(string resourceUri, string eventHubName, string sasToken)
        {
            _eventHubProducerClient = new EventHubProducerClient(resourceUri, eventHubName, new AzureSasCredential(sasToken));
        }

        public async Task SendData(string eventData)
        {
            using var eventBatch = await _eventHubProducerClient.CreateBatchAsync();
            eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(eventData)));

            await _eventHubProducerClient.SendAsync(eventBatch);
        }
    }
}
