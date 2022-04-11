using System;
using System.Net;
using System.Text;
using System.Threading;
using ServerCore;
namespace DummyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 7777);

            Connector connector = new Connector();

            connector.Connect(endPoint, ()=> { return new ServerSession(); });
            while (true)
            {
                ;
            }
        }
    }
}
