﻿using System;
using System.Net;
using System.Text;
using System.Threading;
using ServerCore;

namespace Server
{
    
    class Program
    {

        static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 7777);


            _listener.Init(endPoint, () => { return new ClientSession(); });
            Console.WriteLine("Listening....");


            //프로그램이 끝나지만 않게 대기
            while (true)
            {
                ;
            }
        }
    }
}
