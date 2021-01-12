#
This Azure Function is a Service Bus Trigger. It reads messages from Service Bus as soon as they arrive.
I prefer to read in batches of 100. I also prefer my caller who is writing into the queue use sessionId attribute in their message, so they will maintain an order sequence.
I am processing 500 messages per second by running this function locally inside my context of Visual Studio 2019. If my client is sendind more than 15K of data per message, then my processing speed will be lower.

#
By default, the Function runtime receives a message in a PeekLock mode. It calls Complete on the message if the function finishes successfully (no exceptions), or calls Abandon if the function fails. By default autoComplete is set to true on the service bus settings. 
#
When set to false, you are responsible to call MessageReceiver methods to complete, abandon or deadletter the message. Once the lock expires, the message is re-queued with the DeliveryCount incremented and the lock is automcatically released and renewed.

For every message, data is written into a Blob Storage. I am using autoComplete=false in my host.json. Hence I have to acknowledge every message so that it is dequeued!
You will see this on line #51 of the Function1.cs as I wanted to take control and my critical code inside a Transaction context.

You are welcome to set autoComplete to true. In which case, please comment lines 18,51 and 53 in the Function1.cs

To achieve more througput I have deployed this Az Function into an App Service Plan with 3 instances of PremiumV1 and I can consume at the rate of 1000 messages per second. Each instance of the Azure Function works on a separate set of messages, with no contention!

Cheers!
