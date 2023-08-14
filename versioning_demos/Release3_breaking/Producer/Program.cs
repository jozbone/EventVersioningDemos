using Azure.Messaging;
using Azure.Messaging.ServiceBus;
using Producer;

string connectionString = "<sb-connection-string-here>"; ;
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
    var cloudEventv1 = new CloudEvent(
               "/my-org/inovice",
                      nameof(InvoicePostedV1),
                      new InvoicePostedV1
                      {
                          InvoiceNumber = Guid.NewGuid().ToString(), 
                          PurchaseOrder = 12345,
                          Submitter = i / 2 == 0 ? "John Doe" : "Jane Doe"
                      });
    var cloudEventv2 = new CloudEvent(
                      "/my-org/inovice",
                                           nameof(InvoicePostedV2),
                                           new InvoicePostedV2
                                           {
                          InvoiceNumber = Guid.NewGuid().ToString(),
                          PurchaseOrderNumber = 12345,
                          submitter = i / 2 == 0 ? "John Doe" : "Jane Doe"
                      });

    var messagev1 = new ServiceBusMessage(new BinaryData(cloudEventv1))
    {
        ContentType = "application/cloudevents+json",
        ApplicationProperties = { { "Type", cloudEventv1.Type.ToLower() } }
    };
    var messagev2 = new ServiceBusMessage(new BinaryData(cloudEventv2))
    {
        ContentType = "application/cloudevents+json",
        ApplicationProperties = { { "Type", cloudEventv2.Type.ToLower() } }
    };

    Console.WriteLine($"Sending v1 event: {cloudEventv1.Id}");
    Console.WriteLine($"Sending v2 event: {cloudEventv2.Id}");

    // send the message.
    // note: you should use an outbox pattern here to ensure the messages are sent successfully.
    await sender.SendMessageAsync(messagev1);
    await sender.SendMessageAsync(messagev2);
}