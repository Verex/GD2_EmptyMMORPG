using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MMOManager : NetworkManager
{

	public override void OnServerConnect(NetworkConnection connection)
	{
		Debug.LogAssertion(connection.connectionId + " has connected!");
	}
}
