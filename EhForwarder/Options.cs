using CommandLine;

namespace EhForwarder
{
    internal sealed class Options
    {
        [Option('d', "event-data", Required = true, HelpText = "Path to the event data file.")]
        public string? EventData { get; set; }

        [Option('c', "count", Required = true, HelpText = "Number of events to send. Min value=1. Example: 100_000")]
        public int? Count { get; set; }

        [Option('u', "resource-uri", Required = true, HelpText = "Title of the event hub namespace + servicebus.windows.net. Example: evhns.servicebus.windows.net")]
        public string? ResourceUri { get; set; }

        [Option('n', "key-name", Required = true, HelpText = "Shared acces policies -> policy. Example: RootManageSharedAccessKey")]
        public string? KeyName { get; set; }

        [Option('k', "key", Required = true, HelpText = "Primary key.")]
        public string? Key { get; set; }

        [Option('e', "event-hub", Required = true, HelpText = "Name of the event hub. Example: eh")]
        public string? EventHubName { get; set; }
    }
}
