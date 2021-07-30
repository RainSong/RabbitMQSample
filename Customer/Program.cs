using Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consume
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Logger.Info("程序启动");

                BasicCustomer();

            }
            catch (Exception ex)
            {
                Console.WriteLine("未处理的异常，" + ex.Message);
                Logger.Error("未处理的异常", ex);
            }
            Console.ReadKey();
        }

        private static void BasicCustomer()
        {
            var factory = new ConnectionFactory
            {
                HostName = "192.168.198.75",
                Port = 5672,
                VirtualHost = "test",
                UserName = "admin",
                Password = "admin"
            };
            var con = factory.CreateConnection();
            var channel = con.CreateModel();
            channel.QueueDeclare("data_sync", true, false, false, null);
            
            var consume = new EventingBasicConsumer(channel);
            consume.Received += (ch, ea) =>
            {
                var dateNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                try
                {
                    var msg = Encoding.UTF8.GetString(ea.Body.ToArray());
                    Logger.Info($"收到消息: {msg}");
                    Console.WriteLine($"收到消息 {dateNow} : {msg}");
                }
                catch (Exception ex)
                {
                    Logger.Error("读取消息错误", ex);
                    Console.WriteLine($"读取消息错误: {ex.Message}");
                }
            };

            channel.BasicConsume("data_sync", true, consume);
        }
    }
}
