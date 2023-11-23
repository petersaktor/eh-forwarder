using System.Threading.Channels;

namespace EhForwarder
{
    public class Consumer
    {
        private readonly ChannelReader<string> _reader;
        private readonly EhClient _ehClient;
        private readonly string _identifier;

        private int _count = 0;

        public Consumer(ChannelReader<string> reader, EhClient ehClient, string identifier)
        {
            _ehClient = ehClient;
            _reader = reader;
            _identifier = identifier;
        }

        public async Task ConsumeData()
        {
            ArgumentNullException.ThrowIfNull(_ehClient);
            ArgumentNullException.ThrowIfNull(_reader);
            ArgumentNullException.ThrowIfNull(_identifier);

            Console.WriteLine($"CONSUMER ({_identifier}): Starting");

            while (await _reader.WaitToReadAsync())
            {
                if (_reader.TryRead(out var timeString))
                {
                    try
                    {
                        await _ehClient.SendData(timeString);
                        _count++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"CONSUMER ({_identifier}): Exception {ex.Message}");
                    }
                }
            }

            Console.WriteLine($"CONSUMER ({_identifier}): Completed. Messages sent:{_count}");
        }
    }
}
