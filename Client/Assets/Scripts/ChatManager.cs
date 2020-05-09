using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using UnityEngine;
using Tutorial;
using Debug = UnityEngine.Debug;

public class ChatManager : MonoBehaviour
{
    public string ipAddress = "192.168.1.100";
    public int port = 7788;
    private byte[] data = new byte[1024]; //数据容器
    private string message = ""; //接收服务器端的消息容器
    private Socket clientSocket;
    private Thread receiveThread;

    public UIInput textInput;
    public UILabel chatLable;

    private Stopwatch sw; 

    void Start()
    {
        ConnectToServer();
    }

    void Update()
    {
        if (message != null && message != "")
        {
            chatLable.text += "\n" + message;
            message = ""; //清空消息
        }
    }

    void OnDestroy()
    {
        clientSocket.Shutdown(SocketShutdown.Both); //不接收也不发送
        clientSocket.Close(); //释放资源，关闭连接
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
            //message = Encoding.UTF8.GetString(data,0,length);

            if (length > 0)
            {
                TheMsg msg2 = ProtobufferTool.Deserialize<TheMsg>(data);
                message = $"{msg2.Name}说：{msg2.Content}";

                sw.Stop();
                //获取运行时间间隔  
                TimeSpan ts = sw.Elapsed;
                //获取运行时间[毫秒]  
                long times = sw.ElapsedMilliseconds;
                //获取运行的总时间  
                long times2 = sw.ElapsedTicks;
                Debug.Log($"times={times} times2={times2}");
            }
        }
    }

    //Send方法
    void Send(byte[] data)
    {
        UnityEngine.Debug.Log("C2S：" + data.Length);
        clientSocket.Send(data);

        sw = new Stopwatch();
        sw.Start();
    }

    public void OnSendButtonClick()
    {
        string value = textInput.value;

        TheMsg msg = new TheMsg();
        msg.Name = "我";
        msg.Content = textInput.value;
        byte[] data = ProtobufferTool.Serialize(msg);
        Send(data);

        textInput.value = "";
    }
}
