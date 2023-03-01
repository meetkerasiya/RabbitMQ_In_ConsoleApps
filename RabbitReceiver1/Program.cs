using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

factory.ClientProvidedName = "Rabbit Reciever1 App";

IConnection connection = factory.CreateConnection();

IModel channel = connection.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "Demo-Routing-key";
string queueName = "DemoQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);
//Basic quality of service
channel.BasicQos(0, 1, false);
//so prefetchsize=> 0 means we don't care about size of msg
//prefetchCount is how many msg at time in this case 1
//global is to ask if we want same setting in whole system or just
//for current instance so false for that

var consumer=new EventingBasicConsumer(channel);

consumer.Received += (sender, args) =>
{
    var body = args.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Message Recieved: {message}");

    channel.BasicAck(args.DeliveryTag, false);
    
};

string consumerTag = channel.BasicConsume(queueName, false, consumer);

Console.ReadLine();
channel.BasicCancel(consumerTag);
channel.Close();
connection.Close();