using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class RabbitMQUtil
    {
        private static Dictionary<string, IConnection> connections = null;
        private void GetConnection(string userName, string password, string virtualHost, string hostName) 
        {
            var connectionKey = $"{hostName}-{virtualHost}";
            var factory = new ConnectionFactory 
            {
                UserName = userName,
                Password = password,
                VirtualHost = virtualHost,
                HostName = hostName
            };
            var connection = factory.CreateConnection();
        }

        private void SendMessage(string exchangeName) 
        {
            
        }
    }
}
