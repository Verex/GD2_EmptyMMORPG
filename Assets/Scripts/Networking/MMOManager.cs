using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MMOManager : NetworkManager
{

    public override void OnServerConnect(NetworkConnection connection)
    {
        base.OnServerConnect(connection);

        Debug.Log("A client has connected ([" + connection.address + "] Connection ID: " + connection.connectionId + ")");
    }

    public override void OnServerDisconnect(NetworkConnection connection)
    {
        base.OnServerDisconnect(connection);

        Debug.Log("A client has disconnected (Connection ID: " + connection.connectionId + ")");
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);

    }

    private IEnumerator ClientNextConnection(int serverID)
    {
        // Stop current connection.
        StopClient();

        Debug.Log("Connecting to next server");

        yield return new WaitForSeconds(1.0f);

        // Connect to other server.
        ConnectToServer(Handler.ServerConfigurations[serverID]);

        yield break;
    }

    public void ConnectToServer(ServerConfiguration config)
    {
        // Assign values to network manager.
        networkPort = config.port;
        onlineScene = config.onlineSceneName;

        // Start our client.
        StartClient();
    }

    public void ClientMoveServer(int serverID)
    {
        StartCoroutine(ClientNextConnection(serverID));
    }
}
