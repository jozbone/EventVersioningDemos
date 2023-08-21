using Azure.Messaging;
using Azure.Messaging.ServiceBus;
using Common;
using Consumer1;

var config = new DemoServiceBusConsole().BuildConfigurationRoot();
var topicName = config["ServiceBus:TopicName"];
var subscriptionName = config["ServiceBus:Consumer1Subscription"];

// since ServiceBusClient implements IAsyncDisposable we create it with "await using"
await using var client = DemoServiceBusClientFactory.CreateClient(config);

// create a receiver that we can use to receive and settle the message
ServiceBusReceiver receiver = client.CreateReceiver(topicName, subscriptionName);

Console.WriteLine($"{typeof(Program).Assembly.GetName()} - Waiting for messages...");

while (true)
{
// receive the message
    ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

// deserialize the message body into a CloudEvent
    CloudEvent receivedCloudEvent = CloudEvent.Parse(receivedMessage.Body);

// deserialize to our Employee model
    var receivedInvoice = receivedCloudEvent.Data.ToObjectFromJson<InvoicePostedV2>();

    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine($"Received Message Id: {receivedCloudEvent.Id}");
    Console.WriteLine(new string('_', 50));
    Console.WriteLine($"Source: {receivedCloudEvent.Source}");
    Console.WriteLine($"DataSchema: {receivedCloudEvent.DataSchema}");
    Console.WriteLine($"Subject: {receivedCloudEvent.Subject}");
    Console.WriteLine($"Time: {receivedCloudEvent.Time}");
    Console.WriteLine($"Type: {receivedCloudEvent.Type}");
    Console.WriteLine($"Type: {receivedInvoice.InvoiceNumber}");
    Console.WriteLine($"PurchaseOrder Number: {receivedInvoice.PurchaseOrderNumber}");

    // complete the message, thereby deleting it from the service
    await receiver.CompleteMessageAsync(receivedMessage);
}