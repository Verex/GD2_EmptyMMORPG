using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainMenu : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnServerButtonPress()
    {
        System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();

        // Configure the process using the StartInfo properties.
        //process.StartInfo.WorkingDirectory = "C:\\Users\\Zach\\Documents\\GitHub\\GD2_EmptyMMORPG\\Build";
        //process.StartInfo.FileName = "EmptyMMORPG.exe";
        process.StartInfo.Arguments = "-batchmode -nographics -mserver";

        process.Start();
    }
}
