using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientConnector : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

	public void OnConnectPressed()
	{
		// Connect to default server.
		Handler.NetworkManager.ConnectToServer(Handler.ServerConfigurations[0]);
	}
}
