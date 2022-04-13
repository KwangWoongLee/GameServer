using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace Server
{
    
    class Program
    {

        static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            PacketManager.Instance.Register();

            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 7777);

            #region TCP
            _listener.Init(endPoint, () => { return new ClientSession(); });
            Console.WriteLine("Listening....");
            #endregion


            //프로그램이 끝나지만 않게 대기
            while (true)
            {
                ;
            }
        }
    }
}
