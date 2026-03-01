using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Simone.Common.RabbitMQ;
using Simone.Common.RabbitMQ.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddRabbitMQ(builder.Configuration)
    .AddConsumerHandler<ChatMessage>(async (scope, data, token) => 
    {
        Console.WriteLine($"\n[{data.User}]: {data.Message}");
        return (true, null); 
    });

var host = builder.Build();
await host.RunAsync();