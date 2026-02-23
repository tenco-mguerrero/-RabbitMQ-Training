/// <summary>
/// ChatConsumer - RabbitMQ message consumer application
/// This application connects to RabbitMQ and consumes messages from a specific queue.
/// </summary>

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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

// Declare queue (creates it automatically if it doesn't exist)
// - queue: unique queue name
// - durable: false = queue doesn't survive server restarts
// - exclusive: false = queue can be used by multiple connections
// - autoDelete: false = queue is NOT deleted when there are no consumers
await channel.QueueDeclareAsync(
    queue: "chat.queue",
    durable: false,
    exclusive: false,
    autoDelete: false);

// Create an asynchronous consumer to process messages
var consumer = new AsyncEventingBasicConsumer(channel);

/// <summary>
/// Event that executes every time a message is received from the queue
/// </summary>
consumer.ReceivedAsync += async (sender, ea) =>
{
    // Get the message body in byte format
    var body = ea.Body.ToArray();
    
    // Convert bytes to UTF-8 text
    var message = Encoding.UTF8.GetString(body);

    // Display the received message in the console
    Console.WriteLine($"Message received: {message}");

    // Complete the asynchronous task
    await Task.CompletedTask;
};

// Start consuming messages from the queue
// - queue: name of the queue to consume
// - autoAck: true = automatically confirms that the message was received
// - consumer: the consumer that will process the messages
await channel.BasicConsumeAsync(
    queue: "chat.queue",
    autoAck: true,
    consumer: consumer);

Console.WriteLine("Listening for messages...");
Console.ReadLine();  // Keep the application running until Enter is pressed

// Close the channel and connection gracefully
await channel.CloseAsync();
await connection.CloseAsync();