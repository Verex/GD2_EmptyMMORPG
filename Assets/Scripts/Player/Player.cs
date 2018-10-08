using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
	[ClientRpc]
	public void RpcMoveToServer(int serverID)
	{
		Handler.NetworkManager.StopClient();

		
	}

	[Command]
	public void CmdMoveNextServer()
	{

	}
}
