using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class ChatManager : MonoBehaviour
{
    public string ipAddress = "192.168.1.100";
    public int port = 7788;
    public UIInput textInput;
    public UILabel chatLable;

    private Socket clientSocket;
    private Thread receiveThread;
    private byte[] data = new byte[1024]; //数据容器
    private string message = ""; //接收服务器端的消息容器

    void Start()
    {
        ConnectToServer();
    }

    void Update()
    {
        if (message != null && message != "")
        {
            Debug.Log("hello");
            chatLable.text += "\n" + message;
            message = ""; //清空消息
        }
    }

    void ConnectToServer()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //跟服务器端建立连接
        clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), port));

        //创建一个新的线程，用来接收消息
        receiveThread = new Thread(ReceiveMessage);
        receiveThread.Start();
    }

    //这个线程方法用来循环接收消息
    void ReceiveMessage()
    {
        while (true)
        {
            if (!clientSocket.Connected)
            {
                break;
            }
            int length = clientSocket.Receive(data);
            message = Encoding.UTF8.GetString(data,0,length);
            //chatLable.text += "\n" + message; //Unity不能在新线程里操作UI控件
        }
    }

    //Send方法
    void SendMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        clientSocket.Send(data);
    }

    public void OnSendButtonClick()
    {
        string value = textInput.value;
        SendMessage(value);
        textInput.value = "";
    }

    //Unity Stop状态
    void OnDestroy()
    {
        clientSocket.Shutdown(SocketShutdown.Both); //不接收也不发送
        clientSocket.Close(); //释放资源，关闭连接
    }
}
