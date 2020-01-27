/*
 * TCP收发数据前，要先建立连接：服务器端accept，客户端connect。
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace ChatServerTCP
{
    class TCPServer
    {
        public static void BroadcastMessage(byte[] data)
        {
            var notConnectedList = new List<Client>();

            foreach (var client in clientList)
            {
                if (client.Connected)
                {
                    client.Send(data);
                }
                else
                {
                    notConnectedList.Add(client);
                }
            }
            foreach (var temp in notConnectedList)
            {
                clientList.Remove(temp);
            }
        }

        static List<Client> clientList = new List<Client>();
	    static void Main (string[] args)
        {
            Socket tcpServer = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("server is running...");

            tcpServer.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"),7788));

            tcpServer.Listen(100);

            while(true)
            {
                Socket clientSocket = tcpServer.Accept(); //接收连接，返回Socket类
                Console.WriteLine("a client is connected!");
                Client client = new Client(clientSocket); //每个客户端收发消息的逻辑放到Client类中处理
                clientList.Add(client);
            }
        }
    }
}
