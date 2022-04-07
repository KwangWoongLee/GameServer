﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
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

            try
            {
                _listener.Init(endPoint);
                
                while(true)
                {
                    Console.WriteLine("Listening....");

                    Socket clientSocket = _listener.Accept();

                    byte[] recvBuff = new byte[1024];
                    int recvBytes = clientSocket.Receive(recvBuff);
                    string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                    Console.WriteLine($"[From Client] {recvData}");

                    byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to Server");
                    clientSocket.Send(sendBuff);

                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();

                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());

            }

            

        }
    }
}