using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client2 : MonoBehaviour
{
    private UdpClient client;
    private IPEndPoint serverEndPoint;
    private string id;
    // Start is called before the first frame update
    void Start()
    {
        client = new UdpClient(1337);
    }

    // Update is called once per frame
    void Update()
    {
        Receive();
    }

    private void Receive()
	{
        while (client.Available > 0)
        { 
            byte[] receiveBytes = client.Receive(ref serverEndPoint);
            string receiveString = Encoding.ASCII.GetString(receiveBytes);

            string[] args = receiveString.Split(':');
            string cmd = args[0];

            Debug.Log($"Get command {receiveString}");

            switch (cmd)
			{
                case "CONNECTGOOD":
                    id = args[1];
                    LoadMap();
                    break;
                default:
                    Debug.Log($"Get incorrect command {receiveString} from server");
                    break;
			}
		}
	}

    private void LoadMap()
	{
        Debug.Log("Loading multiplayer map...");
	}

    public bool Connect(string name, IPAddress ip, int port)
	{
        try
        {
            serverEndPoint = new IPEndPoint(ip, port);
            client.Connect(serverEndPoint);
            byte[] sendBytes = Encoding.ASCII.GetBytes($"CONNECT:{name}");
            client.Send(sendBytes, sendBytes.Length);
            return true;
        }
        catch (Exception e)
		{
            Debug.Log(e.Message);
            return false;
		}
	}
}
