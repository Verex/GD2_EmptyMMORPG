using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Portal : NetworkBehaviour
{
	[SerializeField] private int toServerID;

	void OnTriggerEnter(Collider other)
	{
		if (!isServer) return;

		// Check if object is player.
		if (other.tag == "Player")
		{
			// Get player component.
			Player player = other.gameObject.GetComponent<Player>();

			player.MoveToServer(toServerID);
		}
	}
}
