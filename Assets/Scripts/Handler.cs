using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class Handler : MonoSingleton<Handler>
{
    protected Handler() { }

    bool customPort = false;

    public int serverID;
    int serverPort;

    public MMOManager networkManager;

    [SerializeField] private GameObject networkManagerPrefab;
    [SerializeField] public List<ServerConfiguration> serverConfigurations;


    public static MMOManager NetworkManager
    {
        get
        {
            return Handler.Instance.networkManager;
        }
    }

    public static int ServerID
    {
        get
        {
            return Handler.Instance.serverID;
        }
    }

    public static List<ServerConfiguration> ServerConfigurations
    {
        get
        {
            return Handler.Instance.serverConfigurations;
        }
    }


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

    Windows.ConsoleWindow console = new Windows.ConsoleWindow();
    Windows.ConsoleInput input = new Windows.ConsoleInput();

    bool isConsoleServer = false;

    void Start()
    {
        // Mark object to not be destroyed.
        DontDestroyOnLoad(gameObject);

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

        // Check if trying to start server.
        if (isConsoleServer)
        {
            // Start console server.
            StartConsoleServer();
        }
        else
        {
            // Load default offline client scene.
            SceneManager.LoadScene("ClientOffline");

            // Instantiate network manager
            GameObject networkManagerObject = Instantiate(networkManagerPrefab);

            // Get network manager component.
            networkManager = networkManagerObject.GetComponent<MMOManager>();

            //networkManager.ConnectToServer(serverConfigurations[0]);
        }
    }

    void StartConsoleServer()
    {
        // Initialize our console.
        console.Initialize();

        // Set initial console title.
        console.SetTitle("Server");

        // Add input listener.
        input.OnInputText += OnInputText;

        // Add output listener.
        Application.logMessageReceived += HandleLog;

        // Check for invalid server configuration.
        if (!(serverID >= 0 && serverID < serverConfigurations.Count))
        {
            Debug.LogError("Invalid server configuration ID.");
            return;
        }

        // Get server configuration for ID.
        ServerConfiguration config = serverConfigurations[serverID];

        // Make sure configuration is not empty.
        if (config == null)
        {
            Debug.LogError("Server configuration not valid (ID: " + serverID + ").");
            return;
        }

        // Check for custom port.
        if (!customPort)
        {
            // Assign default port.
            serverPort = config.port;
        }

        // Assign title for configuration.
        console.SetTitle("(MMO) - " + config.name);

        Debug.LogWarning("Empty MMO RPG Server ~ " + config.name);

        Debug.Log("Configuration ID: " + serverID.ToString() + " on Port: " + serverPort.ToString());

        Debug.Log("Starting server scene \"" + config.onlineSceneName + "\"");

        // Load our offline scene.
        SceneManager.LoadScene("Offline");

        // Instantiate network manager
        GameObject networkManagerObject = Instantiate(networkManagerPrefab);

        // Get network manager component.
        networkManager = networkManagerObject.GetComponent<MMOManager>();

        // Assign values to network manager.
        networkManager.networkPort = serverPort;
        networkManager.onlineScene = config.onlineSceneName;

        // Attempt to start network manager server.
        if (networkManager.StartServer())
        {
            Debug.Log("Network manager spawned and server was started!");
        }
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
        else if (type == LogType.Assert)
            System.Console.ForegroundColor = ConsoleColor.Cyan;
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

    public new void OnDestroy()
    {
        base.OnDestroy();

        if (isConsoleServer)
        {
            console.Shutdown();
        }
    }

#endif
}
