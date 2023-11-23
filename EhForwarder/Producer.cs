using System.Threading.Channels;

namespace EhForwarder
{
    public class Producer
    {
        private readonly ChannelWriter<string> _writer;
        private readonly string _identifier;
        private readonly string _eventData;
        private readonly int _count;

        public Producer(ChannelWriter<string> writer, string eventData, int count, string identifier)
        {
            _writer = writer;
            _eventData = eventData;
            _count = count;
            _identifier = identifier;
        }

        public async Task BeginProducing()
        {
            ArgumentNullException.ThrowIfNull(_writer);
            ArgumentNullException.ThrowIfNull(_eventData);
            ArgumentNullException.ThrowIfNull(_identifier);
            
            Console.WriteLine($"PRODUCER ({_identifier}): Starting");

            for (var i = 0; i < _count; i++)
            {
                await _writer.WriteAsync(_eventData);
            }

            Console.WriteLine($"PRODUCER ({_identifier}): Completed");
        }
    }
}
