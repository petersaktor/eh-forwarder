using CommandLine;
using EhForwarder;

Console.WriteLine($"EhForwarder start {getTime()}.");

try
{
    var parser = new Parser(with => with.HelpWriter = Console.Out);
    if (args.Length == 0)
    {
        parser.ParseArguments<Options>(new[] { "--help" });
        return;
    }

    var result = parser.ParseArguments<Options>(args)
        .WithParsed(async options =>
        {
            await Forwarder.RunAsync(options);
        })
        .WithNotParsed((errs) => HandleParseError(errs));

    if (result.Errors.Any())
    {
        Console.WriteLine($"EhForwarder finished with error. {getTime()}.");
    }
    else
    {
        Console.WriteLine($"EhForwarder finished successfully {getTime()}.");
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.WriteLine($"EhForwarder finished with execption {getTime()}.");
}

static void HandleParseError(IEnumerable<Error> errs)
{
    Console.WriteLine("EhForwarder failed to parse arguments.");
    Console.WriteLine(errs);
}

static string getTime()
{
    return DateTime.Now.ToString("h:mm:ss tt");
}


