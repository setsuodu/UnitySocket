using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class ChatManager : MonoBehaviour
{
    public string ipAddress = "192.168.1.100";
    public int port = 7788;
    public UIInput textInput;

    private Socket clientSocket;

	void Start ()
    {
        ConnectToServer();
    }
	
	void Update ()
    {
		
	}

    void ConnectToServer()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //跟服务器端建立连接
        clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), port));
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
    }
}
