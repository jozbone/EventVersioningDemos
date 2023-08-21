namespace Common;

public class DemoServiceBusClientFactory
{
    public static ServiceBusClient CreateClient(IConfigurationRoot config)
    {
        // get the namespace and topic from the shared settings
        var sbNamespace = config["ServiceBus:Namespace"];

        // Refer to the following article for more information on setting up the Azure Service Bus client and using managed identities:
        // https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues?tabs=passwordless

        var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            // Disabling the managed identity credential to attempt to speed things up locally.
            // This demo isn't expected to be run in production.
            ExcludeManagedIdentityCredential = true
        });


        return new ServiceBusClient(sbNamespace, credential);
    }
}

/// <summary>
/// Dumb little class to make demo scaffolding a little faster
/// </summary>
public class DemoServiceBusConsole
{
    public IConfigurationRoot BuildConfigurationRoot()
    {
        var sharedFolder = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Shared");
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Path.Combine(sharedFolder, "SharedSettings.json"), optional: false)
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        return config;
    }
}