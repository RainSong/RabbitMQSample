using Microsoft.VisualStudio.TestTools.UnitTesting;
using Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Provider.Tests
{
    [TestClass()]
    public class HelloRabbitMQTests
    {
        private readonly HelloRabbitMQ mqHello = new HelloRabbitMQ();
        [TestMethod()]
        public void HelloTest()
        {
            mqHello.Hello();
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void SendConfirmTest()
        {
            ulong ackTag = 0;
            ulong nackTag = 0;
            (ackTag,nackTag)  = mqHello.SendConfirm();

            Console.WriteLine("AckTag:" + ackTag);
            Console.WriteLine("NackTag:" + nackTag);
            Assert.IsTrue(true);
        }
    }
}