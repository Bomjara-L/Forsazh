﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Source;
using System;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
	public TcpClient client;
	private StreamReader reader;
	private StreamWriter writer;
	private NetworkStream stream;

	private bool onServer = false;
	private string id;
	private Server server;

	private GameObject myVehicle = null;
	private Vector3 myPosition;
	private Quaternion myRotation;

	private GameObject playersObject;

	private List<ClientConnection> clientList;

	private Thread networkThread;
	private Thread sendThread;

	// Start is called before the first frame update
	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		client = new TcpClient();
		Debug.developerConsoleVisible = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (client.Connected)
		{
			if (onServer && SceneManager.GetActiveScene().buildIndex != 6)
			{
				LoadLevel();
			}
			else if (myVehicle != null)
			{
				myPosition = myVehicle.transform.position;
				myRotation = myVehicle.transform.rotation;
				foreach (ClientConnection _client in clientList)
				{
					if (_client.spawned && _client.vehicle == null)
					{
						_client.vehicle = server.SpawnPlayerVehicle();
						_client.vehicle.name = _client.id.ToString();
						Transform playerNickObject = _client.vehicle.gameObject.transform.Find("PlayerNick");
						if (playerNickObject != null)
						{
							playerNickObject.GetComponent<TextMesh>().text = _client.nickname;
						}

					}
					_client.vehicle.transform.position = _client.position;
					_client.vehicle.transform.Rotate(new Vector3(_client.rotation.x, _client.rotation.y, _client.rotation.z));
				}
			}
		}
	}

	private void LoadLevel()
	{
		SceneManager.LoadScene(6);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.buildIndex == 6 && client.Connected)
		{
			writer.WriteLine("LOADED");
		}
		server = GameObject.Find("Server").gameObject.GetComponent<Server>();
		playersObject = GameObject.Find("Players");
		myVehicle = GameObject.Find("car_5");
		sendThread = new Thread(new ParameterizedThreadStart(SendTask));
		sendThread.Start();
	}

	private void SendTask(object obj)
	{
		while (true)
		{
			if (client.Connected && myVehicle != null)
			{
				string posLine = string.Format("MYPOS:{0}:{1}:{2}:{3}", id, myPosition.x, myPosition.y, myPosition.z);
				string rotLine = string.Format("MYROT:{0}:{1}:{2}:{3}", id, myRotation.x, myRotation.y, myRotation.z);
				writer.WriteLine(posLine);
				writer.WriteLine(rotLine);
			}
			Thread.Sleep(20);
		}
	}

	private void NetworkTask(object obj)
	{
		while (true)
		{
			if (stream.DataAvailable)
			{
				string line = reader.ReadLine();
				if (line != null)
				{
					string[] command = line.Split(':');
					string cmd = command[0].ToUpperInvariant();
					switch (cmd)
					{
						case "GOTOME":
							onServer = true;
							id = command[1];
							break;
						case "NEWPLAYERSPAWN":
							ClientConnection newPlayer = new ClientConnection
							{
								id = int.Parse(command[1]),
								spawned = true,
								nickname = command[2]
							};
							clientList.Add(newPlayer);
							break;
						case "EXISTSPLAYER":
							ClientConnection existsPlayer = new ClientConnection
							{
								id = int.Parse(command[1]),
								spawned = true,
								nickname = command[2]
							};
							clientList.Add(existsPlayer);
							break;
						case "POS":
							ClientConnection cl = clientList.Find(c => c.id == int.Parse(command[1]));
							if (cl != null)
							{
								cl.position = new Vector3(float.Parse(command[2]), float.Parse(command[3]), float.Parse(command[4]));
							}
							break;
						case "ROT":
							ClientConnection clRot = clientList.Find(c => c.id == int.Parse(command[1]));
							if (clRot != null)
							{
								clRot.rotation = new Quaternion(float.Parse(command[2]), float.Parse(command[3]), float.Parse(command[4]), 0f);
							}
							break;
						default:
							Debug.LogErrorFormat("Client Get Wrong Command: {0}", cmd);
							break;
					}
				}
			}
		}
	}

	public bool ConnectToServer(string name, string ip, string port)
	{
		IPEndPoint _ip = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
		client.Connect(_ip);
		if (client.Connected)
		{
			stream = client.GetStream();
			reader = new StreamReader(stream);
			writer = new StreamWriter(stream)
			{
				AutoFlush = true
			};
			string connectLine = string.Format("CONNECTPLEASE:{0}", name);
			writer.WriteLine(connectLine);
			clientList = new List<ClientConnection>();
			networkThread = new Thread(new ParameterizedThreadStart(NetworkTask))
			{
				IsBackground = true
			};
			networkThread.Start();
		}
		return client.Connected;
	}
}