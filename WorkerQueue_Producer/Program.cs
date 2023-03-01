using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Text;
using System.Xml;

public class program
{
    public static ConnectionFactory _factory;
    public static IConnection _connection;
    public static IModel _model;

    private const string QueueName = "WorkerQueue_Queue";

    static void Main(string[] args)
    {
        var payment1 = new Payment { AmountToPay = 25.0m, CardNumber = "64564646845645646" };
        var payment2 = new Payment { AmountToPay = 2.0m, CardNumber = "56468464684684684" };
        var payment3 = new Payment { AmountToPay = 18.0m, CardNumber = "33513515315315315" };
        var payment4 = new Payment { AmountToPay = 5.0m, CardNumber = "56186153453353131" };
        var payment5 = new Payment { AmountToPay = 3.0m, CardNumber = "61861618134343513" };
        var payment6 = new Payment { AmountToPay = 91.0m, CardNumber = "7834534813133843" };
        var payment7 = new Payment { AmountToPay = 10.0m, CardNumber = "64564646845645646" };
        var payment8 = new Payment { AmountToPay = 16.0m, CardNumber = "84494595953131212" };
        var payment9 = new Payment { AmountToPay = 25.0m, CardNumber = "64845123264848484" };
        var payment10 = new Payment { AmountToPay = 23.0m, CardNumber = "98989745465464312" };

        CreateConnection();

        SendMessage(payment1);
        SendMessage(payment2);
        SendMessage(payment3);
        SendMessage(payment4);
        SendMessage(payment5);
        SendMessage(payment6);
        SendMessage(payment7);
        SendMessage(payment8);
        SendMessage(payment9);
        SendMessage(payment10);

        Console.ReadLine();
    }

    private static void CreateConnection()
    {
        _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
        _connection = _factory.CreateConnection();
        _model = _connection.CreateModel();

        _model.QueueDeclare(QueueName, true, false, false, null);
    }

    private static void SendMessage(Payment message)
    {
        BinaryFormatter b=new();
        _model.BasicPublish("", QueueName, null, Encoding.UTF8.GetBytes( JsonConvert.SerializeObject(message)));
        Console.WriteLine(" Payment Sent {0}, £{1}", message.CardNumber, message.AmountToPay);
    }
}


