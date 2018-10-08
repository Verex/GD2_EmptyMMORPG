using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MMOManager : NetworkManager
{

	public override void OnClientConnect(NetworkConnection connection)
	{
		base.OnClientConnect(connection);

		Debug.Log(connection.connectionId + " has connected!");
	}
}
