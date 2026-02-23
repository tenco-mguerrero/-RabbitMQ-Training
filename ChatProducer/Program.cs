/// <summary>
/// ChatProducer - RabbitMQ message producer application
/// This application connects to RabbitMQ and publishes messages to an exchange 
/// that routes them to a queue using a routing key.
/// </summary>

using System.Text;
using RabbitMQ.Client;

// Create connection factory with RabbitMQ credentials
var factory = new ConnectionFactory()
{
    HostName = "localhost",  // RabbitMQ server
    UserName = "admin",       // User configured in docker-compose
    Password = "admin"        // Password configured in docker-compose
};

// Establish connection with RabbitMQ server
var connection = await factory.CreateConnectionAsync();

// Create a communication channel
var channel = await connection.CreateChannelAsync();

// Declare exchange (message entry point)
// An exchange receives messages from the producer and routes them to queues according to binding rules
// - exchange: exchange name
// - type: Direct = routes messages to queues based on exact routing key
await channel.ExchangeDeclareAsync(
    exchange: "chat.exchange",
    type: ExchangeType.Direct);

// Declare queue (message storage)
// - queue: unique queue name
// - durable: false = queue doesn't survive server restarts
// - exclusive: false = queue can be used by multiple connections
// - autoDelete: false = queue is NOT deleted when there are no consumers
await channel.QueueDeclareAsync(
    queue: "chat.queue",
    durable: false,
    exclusive: false,
    autoDelete: false);

// Binding: link the queue with the exchange using a routing key
// Messages published to the exchange with this routing key will be routed to this queue
// - queue: destination queue name
// - exchange: source exchange name
// - routingKey: key that determines the routing
await channel.QueueBindAsync(
    queue: "chat.queue",
    exchange: "chat.exchange",
    routingKey: "chat.key");

Console.WriteLine("Write messages. Type 'exit' to quit.");

// Main loop to send messages
while (true)
{
    Console.Write("Message: ");
    var message = Console.ReadLine();

    // Check if the user wants to exit
    if (message == "exit")
        break;

    // Convert text message to bytes using UTF-8 encoding
    var body = Encoding.UTF8.GetBytes(message ?? "");

    // Publish the message to the exchange
    // The exchange will route it to the 'chat.queue' queue using the routing key 'chat.key'
    // - exchange: message destination
    // - routingKey: routing key to direct the message
    // - body: message content in bytes
    await channel.BasicPublishAsync(
        exchange: "chat.exchange",
        routingKey: "chat.key",
        body: body);

    Console.WriteLine("Message sent.");
}

// Close the channel and connection gracefully
await channel.CloseAsync();
await connection.CloseAsync();