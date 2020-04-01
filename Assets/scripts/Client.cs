using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
	public TcpClient client;
	private StreamReader reader;
	private StreamWriter writer;
	private NetworkStream stream;

	private bool onServer = false;
	private string id;
	private Server server;
	private GameObject myVehicle;
	private GameObject playersObject;

	// Start is called before the first frame update
	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		client = new TcpClient();
	}

	// Update is called once per frame
	void Update()
	{
		if (client.Connected && stream.DataAvailable)
		{
			string line = reader.ReadLine();
			string[] command = line.Split(':');
			string cmd = command[0].ToUpperInvariant();
			/*string argumentsLine = command.Length > 1 ? line.Substring(command[0].Length + 1) : null;
			string[] arguments = argumentsLine.Split(':');
			if (arguments != null && arguments.Length == 0)
			{
				arguments = null;
			}
			*/
			switch (cmd)
			{
				case "GOTOME":
					LoadLevel();
					id = command[1];
					break;
				case "NEWPLAYERSPAWN":
					GameObject newPlayer = server.SpawnPlayerVehicle();
					newPlayer.gameObject.transform.SetParent(playersObject.transform);
					newPlayer.name = command[1];
					break;
				case "EXISTSPLAYER":
					GameObject otherPlayer = server.SpawnPlayerVehicle();
					otherPlayer.gameObject.transform.SetParent(playersObject.transform);
					otherPlayer.name = command[1];
					break;
				case "POS":
					break;
				case "ROT":
					break;
				default:
					Debug.LogErrorFormat("Client Get Wrong Command: {0}", cmd);
					break;
			}
		}
		if (client.Connected && myVehicle != null)
		{
			Vector3 pos = myVehicle.transform.position;
			Quaternion rot = myVehicle.transform.rotation;
			string posLine = string.Format("MYPOS:{0}:{1}:{2}:{3}", id, pos.x, pos.y, pos.z);
			string rotLine = string.Format("MYROT:{0}:{1}:{2}:{3}", id, rot.x, rot.y, rot.z);
			writer.WriteLine(posLine);
			writer.WriteLine(rotLine);
			Debug.LogFormat("My position: {0}", pos.ToString());
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
	}

	public bool ConnectToServer(string name, string ip, string port)
	{
		IPEndPoint _ip = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
		client.Connect(_ip);
		if (client.Connected)
		{
			stream = client.GetStream();
			reader = new StreamReader(stream);
			writer = new StreamWriter(stream);
			writer.AutoFlush = true;
			string connectLine = string.Format("CONNECTPLEASE:{0}", name);
			writer.WriteLine(connectLine);
		}
		return client.Connected;
	}
}
