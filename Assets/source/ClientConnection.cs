using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Source
{
	public class ClientConnection
	{
		public TcpClient client;
		public NetworkStream stream;
		public StreamReader reader;
		public StreamWriter writer;
		public string nickname;
		public Vector3 position;
		public Quaternion rotation;
		public bool spawned = false;
		public GameObject vehicle;
		public int id;
	}
}
