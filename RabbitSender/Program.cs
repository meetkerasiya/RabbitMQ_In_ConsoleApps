using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

factory.ClientProvidedName = "Rabbit Sender App";

IConnection connection = factory.CreateConnection();

IModel channel=connection.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "Demo-Routing-key";
string queueName = "DemoQueue";

channel.ExchangeDeclare(exchangeName,ExchangeType.Direct);
channel.QueueDeclare(queueName,  false, false,  false, null);
channel.QueueBind(queueName,exchangeName, routingKey, null);

//byte[] messageBodyBytes = Encoding.UTF8.GetBytes("Namaste again");
//channel.BasicPublish(exchangeName,routingKey,null,messageBodyBytes);

for(int i = 0; i < 60; i++)
{
    Console.WriteLine($"Sending Message {i}");
    byte[] messageBodyBytes = Encoding.UTF8.GetBytes($"Message #{i}");
    Thread.Sleep(1000);
    
}

channel.Close();
connection.Close();