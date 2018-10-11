using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CoinPickup : NetworkBehaviour
{
	void OnCollisionEnter(Collision collision)
	{
		if (!isServer) return;

		// Check if we collided with player.
		if (collision.collider.tag == "Player")
		{
			Player player = collision.collider.gameObject.GetComponent<Player>();
			player.AddScore(1);

			NetworkServer.Destroy(this.gameObject);
		}
	}
}
