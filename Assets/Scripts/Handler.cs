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

    bool consoleCreated = false;

    void Awake()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        string inputString = "";
        for (int i = 0; i < args.Length; i++)
        {
            Debug.Log("ARG " + i + ": " + args[i]);
            if (args[i] == "-mserver")
            {
                StartConsoleServer();
            }
        }

		if (!consoleCreated)
		{
			SceneManager.LoadScene("Offline");
		}
    }

    void StartConsoleServer()
    {
		consoleCreated = true;
        DontDestroyOnLoad(gameObject);

        console.Initialize();
        console.SetTitle("MMORPG Server");

        input.OnInputText += OnInputText;

        Application.logMessageReceived += HandleLog;

        Debug.Log("Server started!");
    }

    void OnInputText(string obj)
    {
        //ConsoleSystem.Run(obj, true);
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
        if (consoleCreated)
        {
            input.Update();
        }
    }

    void OnDestroy()
    {
        if (consoleCreated)
        {
            console.Shutdown();
        }
    }

#endif
}
