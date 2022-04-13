using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HolePunchingTestClient
{
    public partial class Form1 : Form
    {
        Socket udpSocket;
        EndPoint otherClientEndPoint;
        Thread recvThread;

        public Form1()
        {
            InitializeComponent();

            FormClosing += new FormClosingEventHandler(closing);

        }

        public void ServerConnect()
        {
            #region UDP

            recvThread = new Thread(() => {

                while (true)
                {
                    byte[] buffer = new byte[1024];
                    try
                    {
                        if (otherClientEndPoint != null)
                        {
                            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.None, 0);
                            udpSocket.ReceiveFrom(buffer, ref remoteEndPoint);
                            listBox1.Invoke((MethodInvoker)delegate
                            {
                                // Running on the UI thread
                                listBox1.Items.Add(Encoding.UTF8.GetString(buffer));
                            });
                        }
                    }
                    catch (ThreadInterruptedException e)
                    {
                        listBox1.Invoke((MethodInvoker)delegate
                        {
                            // Running on the UI thread
                            listBox1.Items.Add(e.ToString());
                        });
                        break;
                    }

                    catch (SocketException e)
                    {
                        listBox1.Invoke((MethodInvoker)delegate
                        {
                            // Running on the UI thread
                            listBox1.Items.Add(e.ToString());
                        });
                        break;
                    }
                }
                
            });


            //스레드 시작
            recvThread.IsBackground = true;
            recvThread.Start();


            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPAddress serverIp = IPAddress.Parse(textBox3.Text);
            int serverPort = Int32.Parse(textBox4.Text);
            EndPoint serverEndPoint = new IPEndPoint(serverIp, serverPort);
            
            byte[] buffer = Encoding.UTF8.GetBytes("Hi~ Server");
            udpSocket.SendTo(buffer, serverEndPoint);

            buffer = new byte[1024];
            udpSocket.ReceiveFrom(buffer, ref serverEndPoint);

            //받은 내용은 자신의 공인 IP  => TODO : UDP 이므로 못 받을 수도 있으므로 없으면 다시 시도
            string myPublicEndPointStr = Encoding.UTF8.GetString(buffer);
            IPEndPoint myPublicEndPoint = CreateIPEndPoint(myPublicEndPointStr);

            buffer = new byte[1024];
            udpSocket.ReceiveFrom(buffer, ref serverEndPoint);
            string otherPublicEndPointStr = Encoding.UTF8.GetString(buffer);

            IPEndPoint otherPublicEndPoint = CreateIPEndPoint(otherPublicEndPointStr);

            textBox1.Text = otherPublicEndPoint.Address.ToString();
            textBox2.Text = otherPublicEndPoint.Port.ToString();

            otherClientEndPoint = (EndPoint)CreateIPEndPoint($"{textBox1.Text}:{textBox2.Text}");

            #endregion
        }




        public static IPEndPoint CreateIPEndPoint(string endPoint)
        {
            string[] ep = endPoint.Split(':');
            if (ep.Length < 2) throw new FormatException("Invalid endpoint format");
            IPAddress ip;
            if (ep.Length > 2)
            {
                if (!IPAddress.TryParse(string.Join(":", ep, 0, ep.Length - 1), out ip))
                {
                    throw new FormatException("Invalid ip-adress");
                }
            }
            else
            {
                if (!IPAddress.TryParse(ep[0], out ip))
                {
                    throw new FormatException("Invalid ip-adress");
                }
            }
            int port;
            if (!int.TryParse(ep[ep.Length - 1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
            {
                throw new FormatException("Invalid port");
            }
            

            return new IPEndPoint(ip, port);
        }


        public static bool SendToPrivateIP(IPEndPoint otherClient)
        {
            return true;
        }

        public static bool SendToPublicIP(IPEndPoint otherClient)
        {
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServerConnect();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.UTF8.GetBytes($"Hi~ {otherClientEndPoint.ToString()}");

            udpSocket.SendTo(buffer, otherClientEndPoint);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            udpSocket.Close();
        }

        private void closing(object sender, EventArgs e)
        {
            if(udpSocket != null)
                udpSocket.Close();

            Application.Exit();
        }
    }
}
