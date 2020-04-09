using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using Source;
using System.Threading.Tasks;

public class Server : MonoBehaviour
{
	private TcpListener server;
	private IPAddress ip;

	private int id = 1;

	private Client client;
	private bool isServer = false;

	private Thread sendThread;

	private LineSender sender;

	public int port = 1488;
	public int maxSlots = 32;

	public GameObject playerVehicle;
	public GameObject hostVehicle;
	public Vector3 defaultPosition;

	private List<ClientConnection> clientList;

	// Start is called before the first frame update
	void Start()
	{
		client = GameObject.Find("Client").gameObject.GetComponent<Client>();
		if (!client.client.Connected)
		{
			ip = IPAddress.Any;
			clientList = new List<ClientConnection>(maxSlots);
			sender = new LineSender();
			ClientConnection host = new ClientConnection
			{
				id = 9999,
				spawned = true,
				vehicle = hostVehicle,
				nickname = "HOST"
			};
			clientList.Add(host);
			server = new TcpListener(ip, port);
			server.Start();
			isServer = true;
			sendThread = new Thread(new ParameterizedThreadStart(SendNetworkData));
			sendThread.Start();
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
				if (_client.id != 9999) // Check that this player is not the host
				{
					if (_client.spawned && _client.vehicle == null) // Check that player is spawned and spawn him at the host if they not spawned
					{
						_client.vehicle = SpawnPlayerVehicle();
						Transform playerNickObject = _client.vehicle.gameObject.transform.Find("PlayerNick");
						if (playerNickObject != null)
						{
							playerNickObject.GetComponent<TextMesh>().text = _client.nickname;
						}
					}
					if (_client.spawned) // update player vehicle position in the game if they spawned
					{
						_client.vehicle.transform.position = _client.position;
						_client.vehicle.transform.eulerAngles = _client.rotation;

					}
				}
				else
				{
					_client.position = _client.vehicle.transform.position;
					_client.rotation = _client.vehicle.transform.rotation.eulerAngles;
				}
			}
			server.BeginAcceptTcpClient(HandleConnection, server);
		}
	}

	private void SendNetworkData(object obj)
	{
		while (true)
		{
			foreach (ClientConnection client in clientList)
			{
				if (client.id != 9999)
				{
					if (!client.client.Connected) // Delete player if not connected bolshe
					{
						Destroy(client.vehicle);
						clientList.Remove(client);
						continue;
					}
					foreach (ClientConnection _cl in clientList)
					{
						if (_cl != client)
						{
							string text = string.Format("POS:{0}:{1}:{2}:{3}", _cl.id, _cl.position.x, _cl.position.y, _cl.position.z);
							sender.SendMessage(client, text, ref clientList);
							text = string.Format("ROT:{0}:{1}:{2}:{3}", _cl.id, _cl.rotation.x, _cl.rotation.y, _cl.rotation.z);
							sender.SendMessage(client, text, ref clientList);
						}
					}
				}
			}
			Thread.Sleep(10);
		}
	}

	private void ReadNetworkData(object obj)
	{
		ClientConnection _client = (ClientConnection)obj;
		bool active = true;
		while (active)
		{
			if (_client != null && _client.client.Connected)
			{
				if (_client.stream.DataAvailable)
				{
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
								sender.SendMessage(_client, connectResponse, ref clientList);
								break;
							case "LOADED":
								_client.spawned = true;
								foreach (ClientConnection _cl in clientList)
								{
									if (_cl != _client)
									{
										string text = string.Format("NEWPLAYERSPAWN:{0}:{1}", _client.id, _client.nickname);
										if (_cl.id != 9999)
										{
											sender.SendMessage(_cl, text, ref clientList);
										}
										if (_cl.spawned)
										{
											text = string.Format("EXISTSPLAYER:{0}:{1}", _cl.id, _cl.nickname);
											sender.SendMessage(_client, text, ref clientList);
										}
									}
								}
								break;
							case "MYPOS":
								_client.position = new Vector3(float.Parse(command[2]), float.Parse(command[3]), float.Parse(command[4]));
								Debug.LogFormat("Received {0} ID position: {1}", _client.id, _client.position.ToString());
								break;
							case "MYROT":
								_client.rotation = new Vector3(float.Parse(command[2]), float.Parse(command[3]), float.Parse(command[4]));
								Debug.LogFormat("Received {0} ID rotation: {1}", _client.id, _client.rotation.ToString());
								break;
							default:
								Debug.LogFormat("Get Wrong Command From Client: {0}", cmd);
								break;
						}
					}
				}
			}
			else
			{
				active = false;
			}
			Thread.Sleep(5);
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
				id = id,
			};
			newClient.writer.AutoFlush = true;
			newClient.reader.BaseStream.ReadTimeout = 5000;
			newClient.networkThread = new Thread(new ParameterizedThreadStart(ReadNetworkData));
			newClient.networkThread.Start(newClient);
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
}
