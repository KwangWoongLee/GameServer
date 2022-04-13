using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HolePunchingTestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 7777);

            IPAddress recvIp = IPAddress.Parse("0.0.0.0");
            IPEndPoint bindEndPoint = new IPEndPoint(recvIp, 7777);
            Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpSocket.Bind(endPoint);

            Console.WriteLine("UDP Socket Bind");

            ushort count = 1;
            Dictionary<ushort, EndPoint> clientList = new Dictionary<ushort, EndPoint>();

            while (true)
            {
                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.None, 0);


                byte[] buffer = new byte[1024];

                udpSocket.ReceiveFrom(buffer, ref remoteEndPoint);

                Console.WriteLine($"Client : {remoteEndPoint.ToString()}");

                clientList.Add(count++, remoteEndPoint);

                // 이 시점에 클라이언트의 공인IP를 클라이언트에게 전송
                buffer = Encoding.UTF8.GetBytes(remoteEndPoint.ToString());
                udpSocket.SendTo(buffer, remoteEndPoint);

                if (count > 2)
                    break;
            }

            foreach (var client in clientList)
            {
                ushort cnt = client.Key;
                EndPoint clientEndPoint = client.Value;

                foreach (var client2 in clientList)
                {
                    ushort cnt2 = client2.Key;
                    EndPoint client2EndPoint = client2.Value;

                    if (cnt == cnt2)
                        continue;

                    byte[] buffer = Encoding.UTF8.GetBytes(client2EndPoint.ToString());
                    udpSocket.SendTo(buffer, clientEndPoint);
                }
            }


            udpSocket.Close();
        }
    }
}
