﻿using System;
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

    private int id = 1;

    private Client client;
    private bool isServer = false;

    public int port = 8888;
    public int maxSlots = 32;

    public GameObject playerVehicle;
    public Vector3 defaultPosition;

    List<ClientConnection> clientList;

    // Start is called before the first frame update
    void Start()
    {
        client = GameObject.Find("Client").gameObject.GetComponent<Client>();
        //ip = GetAddress();
        if (!client.client.Connected)
        {
            ip = IPAddress.Any;
            clientList = new List<ClientConnection>(maxSlots);
            server = new TcpListener(ip, port);
            server.Start();
            isServer = true;
            Debug.LogFormat("Starting server at {0}:{1}", ip, port);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            foreach (ClientConnection _client in clientList)
            {
                if (!_client.client.Connected || _client.client == null)
                {
                    clientList.Remove(_client);
                    continue;
                }
                if (!_client.stream.DataAvailable)
                {
                    continue;
                }
                string line = _client.reader.ReadLine();
                string response = null;
                string address = _client.client.Client.RemoteEndPoint.ToString();
                Debug.LogFormat("Received line from {0}: {1}", address, line);
                string[] command = line.Split(':');
                string cmd = command[0].ToUpperInvariant();
                if (response == null)
                {
                    switch (cmd)
                    {
                        case "CONNECTPLEASE":
                            _client.nickname = command[1];
                            string connectResponse = string.Format("GOTOME:{0}", _client.id);
                            _client.writer.WriteLine(connectResponse);
                            break;
                        case "LOADED":
                            _client.vehicle = SpawnPlayerVehicle();
                            foreach (ClientConnection _cl in clientList)
                            {
                                if (_cl != _client)
                                {
                                    string text = string.Format("NEWPLAYERSPAWN:{0}:{1}", _client.id, _client.nickname);
                                    _cl.writer.WriteLine(text);
                                    if (_cl.spawned)
                                    {
                                        text = string.Format("EXISTSPLAYER:{0}:{1}", _cl.id, _cl.nickname);
                                        _client.writer.WriteLine(text);
                                    }
                                }
                            }
                            break;
                        case "MYPOS":
                            _client.position = new Vector3(float.Parse(command[2]), float.Parse(command[3]), float.Parse(command[4]));
                            _client.vehicle.transform.position = _client.position;
                            Debug.LogFormat("Received {0} ID position: {1}", _client.id, _client.position.ToString());
                            foreach (ClientConnection _cl in clientList)
                            {
                                if (_cl != _client && _cl.position != null)
                                {
                                    string text = string.Format("POS:{0}:{1}:{2}:{3}", _cl.id, _cl.position.x, _cl.position.y, _cl.position.z);
                                    _client.writer.WriteLine(text);
                                }
                            }
                            break;
                        case "MYROT":
                            _client.rotation = new Quaternion(float.Parse(command[2]), float.Parse(command[3]), float.Parse(command[4]), 0f);
                            _client.vehicle.transform.rotation = _client.rotation;
                            Debug.LogFormat("Received {0} ID rotation: {1}", _client.id, _client.rotation.ToString());
                            foreach (ClientConnection _cl in clientList)
                            {
                                if (_cl != _client && _cl.rotation != null)
                                {
                                    string text = string.Format("ROT:{0}:{1}:{2}:{3}", _cl.id, _cl.rotation.x, _cl.rotation.y, _cl.rotation.z);
                                    _client.writer.WriteLine(text);
                                }
                            }
                            break;
                        default:
                            Debug.LogFormat("Get Wrong Command From Client: {0}", cmd);
                            break;
                    }
                }
            }
            server.BeginAcceptTcpClient(HandleConnection, server);
        }
    }

    public void HandleConnection(IAsyncResult result)
    {
        TcpClient client = server.EndAcceptTcpClient(result);
        Debug.Log("Player connection...");
        NetworkStream ns = client.GetStream();
        if (clientList.Count < clientList.Capacity)
        {
            ClientConnection newClient = new ClientConnection()
            {
                client = client,
                stream = ns,
                reader = new StreamReader(ns),
                writer = new StreamWriter(ns),
                id = id
            };
            newClient.writer.AutoFlush = true;
            clientList.Add(newClient);
            id++;
            Debug.Log("Player connected!");
        }
    }

    public GameObject SpawnPlayerVehicle()
    {
        GameObject newVehicle = Instantiate(playerVehicle, defaultPosition, Quaternion.identity) as GameObject;
        return newVehicle;
    }

    /*
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
    */
}
