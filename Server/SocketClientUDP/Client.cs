using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketServerUDP
{
    class Client
    {
        static void Main(string[] args)
        {
            //创建Socket
            Socket udpClient = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);
            //发送数据
            while (true)
            {
                EndPoint serverPoint = new IPEndPoint(IPAddress.Parse("192.168.1.100"), 7788);
                string message = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(message);
                udpClient.SendTo(data, serverPoint);
            }

            udpClient.Close();
            Console.ReadKey();
        }
    }
}
