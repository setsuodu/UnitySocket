using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System;

namespace ChatServer
{
    class Server
    {
        static List<Client> clientList = new List<Client>();
	    static void Main (string[] args)
        {
            Socket tcpServer = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("server is running...");

            tcpServer.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.100"),7788));

            tcpServer.Listen(100);

            while(true)
            {
                Socket clientSocket = tcpServer.Accept(); //接收连接，返回Socket类
                Client client = new Client(clientSocket); //每个客户端收发消息的逻辑放到Client类中处理
                clientList.Add(client);
            }
        }
    }
}
