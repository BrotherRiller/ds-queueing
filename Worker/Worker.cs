using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.Received += async (model, ea) =>
{
  Console.WriteLine($" [x] Received {message}");

  int dots = message.Split('.').Length - 1;
  await Task.Delay(dots * 1000);

  Console.WriteLine(" [x] Done");
};

await channel.BasicConsumeAsync("hello", autoAck: true, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();