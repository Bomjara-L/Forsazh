using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour
{
    public InputField nameEdit;
    public InputField ipEdit;
    public InputField portEdit;

    public GameObject uiManager;
    public GameObject clientObject;

    private Client client;
    // Start is called before the first frame update
    void Start()
    {
        client = clientObject.gameObject.GetComponent<Client>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnConnectClicked()
    {
        string name = nameEdit.text;
        string ip = ipEdit.text;
        string port = portEdit.text;
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(ip) && !string.IsNullOrEmpty(ip) && !string.IsNullOrEmpty(port) && !string.IsNullOrWhiteSpace(port))
        {
            bool connected = client.ConnectToServer(name, ip, port);
        }
    }
        
}
