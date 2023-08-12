using Azure.Messaging;
using Azure.Messaging.ServiceBus;
using Producer;

string connectionString = "<sb-connection-string>";
string topicName = "versioning";

// since ServiceBusClient implements IAsyncDisposable we create it with "await using"
await using var client = new ServiceBusClient(connectionString);

// create the sender
ServiceBusSender sender = client.CreateSender(topicName);

Console.WriteLine($"{typeof(Program).Assembly.GetName()} - Press any key to start sending messages...");
Console.ReadKey();

for (var i = 0; i < 10; i++)
{
    Thread.Sleep(500);
    // create a payload using the CloudEvent type
    var cloudEvent = new CloudEvent(
               "/myorg/inovice",
                      nameof(InvoicePostedV1),
                      new InvoicePostedV1
                      {
                          InvoiceNumber = Guid.NewGuid().ToString(), 
                          PurchaseOrder = 12345,
                          submitter = i / 2 == 0 ? "John Doe" : "Jane Doe"
                      });
    var message = new ServiceBusMessage(new BinaryData(cloudEvent))
    {
        ContentType = "application/cloudevents+json",
        ApplicationProperties = { { "Type", cloudEvent.Type.ToLower() } }
    };

    Console.WriteLine($"Sending message: {cloudEvent.Id}");
    // send the message
    await sender.SendMessageAsync(message);
}