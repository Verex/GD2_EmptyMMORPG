using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MMOManager : NetworkManager
{
    public Dictionary<int, Transform> playerSpawns;

    private void FindPlayerSpawns()
    {
        playerSpawns = new Dictionary<int, Transform>();

        PlayerSpawn[] spawns = FindObjectsOfType<PlayerSpawn>();

        foreach (PlayerSpawn spawn in spawns)
        {
            playerSpawns.Add(spawn.fromServerID, spawn.transform);
        }
    }

    public override void OnStartServer()
    {
        FindPlayerSpawns();
    }

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

    public void ClientMoveServer(int fromServerID, int serverID)
    {
        Handler.Instance.PlayerData.LastServerID = fromServerID;
        
        StartCoroutine(ClientNextConnection(serverID));
    }
}
