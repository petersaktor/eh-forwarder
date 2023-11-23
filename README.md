# eh-forwarder
Event forwarder to Azure Event Hub command line tool. Send event data to specific Event Hub. It's able to send many events in the short time. Can be used for Load Testing.
  
  -d, --event-data      Required. Path to the event data file.

  -c, --count           Required. Number of events to send. Min value=1. Example: 100_000

  -u, --resource-uri    Required. Title of the event hub namespace + servicebus.windows.net. Example:
                        evhns.servicebus.windows.net

  -n, --key-name        Required. Shared acces policies -> policy. Example: RootManageSharedAccessKey

  -k, --key             Required. Primary key.

  -e, --event-hub       Required. Name of the event hub. Example: eh

  --help                Display this help screen.

  --version             Display version information.
