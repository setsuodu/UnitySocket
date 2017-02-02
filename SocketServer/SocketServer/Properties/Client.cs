﻿using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;

namespace ChatServer
{
    class Client
    {
        private Socket clientSocket;
        private Thread thread;
        private byte[] data = new byte[1024]; //数据容器

        public Client(Socket s)
        {
            clientSocket = s;
            thread = new Thread(ReceiveMessage);
            thread.Start();
        }

        private void ReceiveMessage()
        {
            while (true)
            {
                //在接收数据时判断一下Socket是否端口连接
                if (clientSocket.Poll(10, SelectMode.SelectRead))
                {
                    clientSocket.Close(); //关闭当前连接
                    break; //跳出循环，终止该线程执行
                }

                int length = clientSocket.Receive(data);
                string message = Encoding.UTF8.GetString(data, 0, length); //ToDo: 接收到数据时，要把该数据分发到各个客户端
                //广播这个消息
                Server.BroadcastMessage(message);
                Console.WriteLine("收到了消息：" + message);
            }
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);
        }

        public bool Connected
        {
            get
            {
                return clientSocket.Connected;
            }
        }
    }
}
