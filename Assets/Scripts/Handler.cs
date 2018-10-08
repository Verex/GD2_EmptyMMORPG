using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class Handler : MonoBehaviour
{
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

    Windows.ConsoleWindow console = new Windows.ConsoleWindow();
    Windows.ConsoleInput input = new Windows.ConsoleInput();

    bool isConsoleServer = false;
    bool customPort = false;

    int serverID;
    int serverPort;

    [SerializeField] private GameObject networkManagerPrefab;
    [SerializeField] private int[] defaultPorts;

    private MMOManager networkManager;

    void Awake()
    {
        string[] args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-mserver")
            {
                // Try to get server ID.
                isConsoleServer = Int32.TryParse(args[i + 1], out serverID);
            }
            else if (args[i] == "-mport")
            {
                // Try to get server ID.
                customPort = Int32.TryParse(args[i + 1], out serverPort);
            }
        }

        if (isConsoleServer)
        {
            // Start console server.
            StartConsoleServer();
        }
        else
        {
            // Load default offline client scene.
            SceneManager.LoadScene("Offline");
        }
    }

    void StartConsoleServer()
    {
        // Mark object to not be destroyed.
        DontDestroyOnLoad(gameObject);

        // Initialize our console.
        console.Initialize();

        // Set console title.
        console.SetTitle("Empty MMORPG Server");

        // Add input listener.
        input.OnInputText += OnInputText;

        // Add output listener.
        Application.logMessageReceived += HandleLog;

        // Handle server IDs.
        switch (serverID)
        {
            case 0:

                break;
            default:
                Debug.LogError("Invalid server ID parameter...");
                return;
        }

        // Check for custom port.
        if (!customPort)
        {
            // Assign default port.
            serverPort = defaultPorts[serverID];
        }

        Debug.LogWarning("Empty MMORPG Server");
        Debug.Log("ID: " + serverID.ToString() + " - Port: " + serverPort.ToString() + "\n");

        Debug.Log("Starting server...");

        // Load our offline scene.
        SceneManager.LoadScene("Offline");

        
    }

    void OnInputText(string obj)
    {
        // HANDLE CONSOLE INPUT.
    }

    void HandleLog(string message, string stackTrace, LogType type)
    {
        if (type == LogType.Warning)
            System.Console.ForegroundColor = ConsoleColor.Yellow;
        else if (type == LogType.Error)
            System.Console.ForegroundColor = ConsoleColor.Red;
        else
            System.Console.ForegroundColor = ConsoleColor.White;

        // We're half way through typing something, so clear this line ..
        if (Console.CursorLeft != 0)
            input.ClearLine();

        System.Console.WriteLine(message);

        // If we were typing something re-add it.
        input.RedrawInputLine();
    }


    void Update()
    {
        if (isConsoleServer)
        {
            input.Update();
        }
    }

    void OnDestroy()
    {
        if (isConsoleServer)
        {
            console.Shutdown();
        }
    }

#endif
}
