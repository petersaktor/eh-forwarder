using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;
using System.Web;

namespace EhForwarder
{
    internal static class Forwarder
    {
        public static async Task RunAsync(Options options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));
            ArgumentNullException.ThrowIfNull(options.EventData, nameof(options.EventData));
            ArgumentNullException.ThrowIfNull(options.Count, nameof(options.Count));
            ArgumentNullException.ThrowIfNull(options.ResourceUri, nameof(options.ResourceUri));
            ArgumentNullException.ThrowIfNull(options.KeyName, nameof(options.KeyName));
            ArgumentNullException.ThrowIfNull(options.Key, nameof(options.Key));
            ArgumentNullException.ThrowIfNull(options.EventHubName, nameof(options.EventHubName));

            if (options.Count < 1)
            {
                throw new ArgumentException("Count must be greater than 0.");   
            }

            var nrOfConsumers = int.Min(options.Count.Value, 8);

            string eventData = File.ReadAllText(options.EventData);
            
            var sasToken = CreateToken(options.ResourceUri, options.KeyName, options.Key);

            var channel = Channel.CreateBounded<string>(new BoundedChannelOptions(20_000)
            {
                SingleWriter = true,
                SingleReader = false,
                AllowSynchronousContinuations = false,
                FullMode = BoundedChannelFullMode.Wait,
            });

            var producer = new Producer(channel.Writer, eventData, options.Count.Value, "Producer");

            var consumerTasks = Enumerable
                .Range(1, nrOfConsumers)
                .Select(consumerNumber =>
                {
                    var consumer = new Consumer(channel.Reader, new EhClient(options.ResourceUri, options.EventHubName, sasToken), $"Consumer{consumerNumber}");
                    return consumer.ConsumeData();  // begin consuming
                })
                .ToList();

            Task producerTask = producer.BeginProducing(); // begin producing

            await producerTask.ContinueWith(t => channel.Writer.TryComplete(t.Exception));

            await Task.WhenAll(consumerTasks);
        }

        private static string CreateToken(string resourceUri, string keyName, string key)
        {
            TimeSpan sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var week = 60 * 60 * 24 * 7;
            var expiry = Convert.ToString((int)sinceEpoch.TotalSeconds + week);
            string stringToSign = HttpUtility.UrlEncode(resourceUri) + "\n" + expiry;
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var sasToken = String.Format(CultureInfo.InvariantCulture, "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}", HttpUtility.UrlEncode(resourceUri), HttpUtility.UrlEncode(signature), expiry, keyName);
            return sasToken;
        }
    }
}
