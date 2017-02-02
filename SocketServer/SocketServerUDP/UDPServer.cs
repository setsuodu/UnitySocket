/*
 * UDP不需要事先建立连接，直接根据IP，Port连。
 */
 using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using System.Threading;

namespace ChatServerUDP
{
    class UDPServer
    {
        private static Socket udpServer;

        static void Main(string[] args)
        {
            //1，创建Socket
            udpServer = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);

            //2，绑定IP,Port
            udpServer.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.100"),7788));

            //3，接收数据，使用线程循环执行
            new Thread(ReceiveMessage) { IsBackground = true }.Start();

            Console.ReadKey(); //暂停程序
        }

        static void ReceiveMessage()
        {
            while (true) //循环处理内部代码
            {
                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0); //表示不需要赋值
                byte[] data = new byte[1024];

                //这个方法会把数据来源（IP，Port），放到第二个参数上。
                int length = udpServer.ReceiveFrom(data, ref remoteEndPoint); //ref表示可以对该变量的属性做修改
                string message = Encoding.UTF8.GetString(data, 0, length);
                Console.WriteLine("从IP：" + (remoteEndPoint as IPEndPoint).Address.ToString() + " : " + (remoteEndPoint as IPEndPoint).Port.ToString() + "收到了数据：" + message);
            }
        }
    }
}
