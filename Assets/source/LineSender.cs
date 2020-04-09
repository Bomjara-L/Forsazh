using Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Source
{
	class LineSender
	{
		public void SendMessage(ClientConnection client, string message, ref List<ClientConnection> clientList)
		{
			try
			{
				client.writer.WriteLine(message);
			}
			catch (Exception e)
			{
				Debug.LogWarning(e.Message);
				client.client.Close();
				clientList.Remove(client);
				throw;
			}
		}

		public void SendMessage(StreamWriter writer, string message)
		{
			try
			{
				writer.WriteLine(message);
			}
			catch (Exception e)
			{
				Debug.LogWarning(e.Message);
				throw;
			}
		}
	}
}
