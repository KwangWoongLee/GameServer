using System;
using System.Net;
using System.Text;
using System.Threading;
using ServerCore;

namespace Server
{
    public class Packet
    {
        public ushort size;
        public ushort id;
    }
    class GameSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"Connected by : {endPoint}");

            //Packet packet = new Packet() { size = 4, id = 1 };

            //byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome To Server");


            //ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);
            //byte[] buffer = BitConverter.GetBytes(packet.size);
            //byte[] buffer2 = BitConverter.GetBytes(packet.id);
            //Array.Copy(buffer, 0, openSegment.Array, openSegment.Offset, buffer.Length);
            //Array.Copy(buffer2, 0, openSegment.Array, openSegment.Offset + buffer.Length, buffer2.Length);

            //SendBufferHelper.Close(packet.size);

            //Send(sendBuff);

            Thread.Sleep(5000);
            Disconnect();
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes : {numOfBytes}");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"Disconnected by : {endPoint}");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + 2);
            Console.WriteLine($"RecvPacket Size : {size} , Id : {id}");
        }
    }
    class Program
    {

        static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 7777);


            _listener.Init(endPoint, () => { return new GameSession(); });
            Console.WriteLine("Listening....");


            //프로그램이 끝나지만 않게 대기
            while (true)
            {
                ;
            }
        }
    }
}
