using System;
using System.Collections.Generic;
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
		public string nickname;
		public Vector3 position;
	}
}
