using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Simone.Common.RabbitMQ;
using Simone.Common.RabbitMQ.Extensions;
using System.Text.Json;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddRabbitMQ(builder.Configuration);
var host = builder.Build();

_ = host.RunAsync();

var rabbitManager = host.Services.GetFirstRabbitMqManager();

Console.Write("Tu nombre: ");
var user = Console.ReadLine();

while (true)
{
    Console.Write("Mensaje: ");
    var text = Console.ReadLine();
    
    if (string.IsNullOrEmpty(text)) continue;

    var msg = new ChatMessage { 
        User = user ?? "Anon", 
        Message = text,
        CorrelationId = Guid.NewGuid().ToString() 
    };

    var json = JsonSerializer.Serialize(msg);
    await rabbitManager.PublishMessage(json);
    Console.WriteLine("Sent!");
}