using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;

public class Program
{
    private static ConnectionFactory _factory;
    private static IConnection _connection;

    private const string QueueName = "WorkerQueue_Queue";

    static void Main()
    {
        Receive();

        Console.ReadLine();
    }

    public static void Receive()
    {
        _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
        _connection = _factory.CreateConnection();

        var channel = _connection.CreateModel();
            
                channel.QueueDeclare(QueueName, true, false, false, null);
                channel.BasicQos(0, 1, false);
               
                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, args) =>
                {
                    Task.Delay(1000).Wait();    
                    var body = args.Body.ToArray();
                    var message = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(body));
                    Console.WriteLine(message);
                    channel.BasicAck(args.DeliveryTag, false);

                };
                channel.BasicConsume(QueueName, false, consumer);

            

        
    }
}
