using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
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
            //나의 사설 ip
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 7777);

            #region TCP
            Connector connector = new Connector();

            connector.Connect(endPoint, () => { return new ServerSession(); });
            #endregion


            while (true)
            {
                ;
            }
        }

    }
}
