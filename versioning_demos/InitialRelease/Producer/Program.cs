var config = new DemoServiceBusConsole().BuildConfigurationRoot();
var topicName = config["ServiceBus:TopicName"];

await using var client = DemoServiceBusClientFactory.CreateClient(config);

// create the sender
ServiceBusSender sender = client.CreateSender(topicName);

Console.WriteLine($"{typeof(Program).Assembly.GetName()} - Press any key to start sending messages...");
Console.ReadKey();

for (var i = 0; i < 10; i++)
{
    // create a payload using the CloudEvent type
    var cloudEvent = new CloudEvent(
               "/myorg/inovice",
                      nameof(InvoicePostedV1),
                      new InvoicePostedV1 { InvoiceNumber = Guid.NewGuid().ToString(), PurchaseOrder = 12345 });
    var message = new ServiceBusMessage(new BinaryData(cloudEvent))
    {
        ContentType = "application/cloudevents+json",
        ApplicationProperties = { { "Type", cloudEvent.Type.ToLower() } }
    };

    Console.WriteLine($"Sending message: {cloudEvent.Id}");
    // send the message
    await sender.SendMessageAsync(message);
}