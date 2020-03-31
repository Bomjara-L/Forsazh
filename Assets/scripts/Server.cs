using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using Source;

public class Server : MonoBehaviour
{
    private TcpListener server;
    private IPAddress ip;

    public int port = 8888;
    public int maxSlots = 32;

    public GameObject playerVehicle;
    public Vector3 defaultPosition;

    List<ClientConnection> clientList;

    // Start is called before the first frame update
    void Start()
    {
        //ip = GetAddress();
        ip = IPAddress.Any;
        clientList = new List<ClientConnection>(maxSlots);
        server = new TcpListener(ip, port);
        server.Start();
        Debug.LogFormat("Starting server at {0}:{1}", ip, port);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (ClientConnection _client in clientList)
        {

        }
        server.BeginAcceptTcpClient(HandleConnection, server);
    }

    private void HandleConnection(IAsyncResult result)
    {
        TcpClient client = server.EndAcceptTcpClient(result);
        Debug.Log("Player connection...");
        NetworkStream ns = client.GetStream();
        if (clientList.Count < clientList.Capacity)
        {
            ClientConnection newClient = new ClientConnection()
            {
                client = client,
                stream = ns
            };
            clientList.Add(newClient);
            SpawnPlayerVehicle();
            Debug.Log("Player connected!");
        }
    }

    private void SpawnPlayerVehicle()
    {
        GameObject newVehicle = Instantiate(playerVehicle, defaultPosition, Quaternion.identity);
        newVehicle.transform.position = defaultPosition;
    }

    private IPAddress GetAddress()
    {
        string address;
        WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
        using (WebResponse response = request.GetResponse())
        {
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }
            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);
            return IPAddress.Parse(address);
        }
    }
}
