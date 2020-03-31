using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerMenu : MonoBehaviour
{
    TcpClient client;
    // Start is called before the first frame update
    void Start()
    {
        client = new TcpClient();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
