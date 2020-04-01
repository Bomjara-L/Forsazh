using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MultiplayerMenu : MonoBehaviour
{
    private TcpClient client;
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
