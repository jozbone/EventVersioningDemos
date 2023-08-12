# Event Versioning Demos
Simple demos showing different event versioning scenarios. The sample all leverage Azure Service Bus.
The samples are all simple console apps with a single publisher to a topic and two subscribers to individual subscriptions under the topic.

## Initial Release
Shows a simple scenario where the publisher publishes a v1 event, which is consumed by both subscribers using their own subscriptions.

## Release 2 - non-breaking change
Demo of a non-breaking change (adding a property to the event). The following changes were made to the *Initial Release* sample.
- The publisher now publishes the event with an additinoal property, which represents a non-breaking change. The version doesn't change in this case. It is still a v1 event.
- One subscriber only knows about the original event, but still continutes to work just fine.
- The other subscriber knows about the modified event and can take advantage of the additional property in the payload.

## Release 3 - Breaking change
Demo of a breaking change due to changing the name of the *PurchaseOrder* property to *PurchaseOrderName*. 
The publisher know has to make sure it doesn't break existing subscribers, while also allowing subscribers to update to consuming the new event on their own release schedule. To do this, the publish will perform a dual write (aka double publish) of the v1 and v2 events. This allows the consumers to eventually consume the new v2 event, after which the publisher can potentially stop publishing the v1 event.

The following changes were made to the *Release 2* sample.
- The publisher now publishes a v2 version of the event, which represents a breaking change.
- One subscriber only knows about the v1 event. It can use a subscription filter to filter out any other events. Since the subscriber will only receive the v1 event it will still continute to work as expected. It never receives the v2 event.
- The other subscriber knows about the v2 event and can consumer it. This subscriber would use a subscription filter as well to only receive the v2 event.

These demos just leverage the Azure Service Bus subscription filters, but you can obviously achieve similar filtering with other technologies like Dapr pub/sub with CEL and other options instead.
