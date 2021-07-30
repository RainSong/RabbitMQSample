using System;
using System.Diagnostics;
using System.Text;

using RabbitMQ.Client;

namespace Provider
{
    public class HelloRabbitMQ
    {
        private const string MQ_HOST_NAME = "192.168.198.75";
        private const string MQ_VITRUAL_HOST_NAME = "test";
        private const int MQ_PORT = 5672;
        private const string MQ_USER_NAME = "admin";
        private const string MQ_USER_PASSWORD = "admin";
        public void Hello()
        {
            var conFacotry = new ConnectionFactory
            {
                HostName = MQ_HOST_NAME,
                Port = MQ_PORT,
                VirtualHost = MQ_VITRUAL_HOST_NAME,
                UserName = MQ_USER_NAME,
                Password = MQ_USER_PASSWORD
            };
            using (var connection = conFacotry.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    /*
                        Queue       队列名称
                        durable     是否持久化
                        exclusive   是否独有
                        autoDelete  是否自动删除，订阅者消费后是否自动删除
                     */
                    channel.QueueDeclare("data_sync", true, false, false, null);
                    //channel.ExchangeDeclare("", ExchangeType.Direct, true, null);

                    var msg = $"Hello RabbitMQ [Send Time:{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]";
                    var bytes = Encoding.UTF8.GetBytes(msg);

                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2;

                    channel.BasicPublish("", "data_sync", false, properties, bytes);
                }
            }
        }

        public (ulong, ulong) SendConfirm()
        {
            var facotry = new ConnectionFactory
            {
                HostName = MQ_HOST_NAME,
                Port = MQ_PORT,
                VirtualHost = MQ_VITRUAL_HOST_NAME,
                UserName = MQ_USER_NAME,
                Password = MQ_USER_PASSWORD
            };

            ulong ackTag = 0;
            ulong nackTag = 0;
            using (var con = facotry.CreateConnection())
            {
                using (var channel = con.CreateModel())
                {
                    channel.QueueDeclare("data_sync", true, false, false, null);
                    //设置前的channel处于确认模式
                    channel.ConfirmSelect();
                    
                    channel.BasicAcks += (o, args) =>
                    {
                        ackTag = args.DeliveryTag;
                    };

                    channel.BasicNacks += (o, args) => 
                    {
                        nackTag = args.DeliveryTag;
                    };

                    var msg = $"Hello RabbitMQ,With publish confirm [Send Time:{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]";
                    var bytes = Encoding.UTF8.GetBytes(msg);

                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2;
                    properties.Persistent = true;

                    channel.BasicPublish("", "data_sync", false, properties, bytes);
                    if (channel.WaitForConfirms()) 
                    {
                        
                    }

                }
            }
            return (ackTag, nackTag);
        }
    }
}
