using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientConnector : MonoBehaviour
{
    [SerializeField] private InputField usernameInputField;

	public void OnConnectPressed()
	{
        // Reset our player data.
        Handler.Instance.PlayerData.Reset();

        // Assign username
        Handler.Instance.PlayerData.Username = usernameInputField.text;

		// Connect to default server.
		Handler.NetworkManager.ConnectToServer(Handler.ServerConfigurations[0]);
	}
}
