using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

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

        System.Diagnostics.Process process = new System.Diagnostics.Process();
        // Configure the process using the StartInfo properties.
        process.StartInfo.WorkingDirectory = "C:\\Users\\Zach\\Documents\\GitHub\\GD2_EmptyMMORPG\\Build";
        process.StartInfo.FileName = "EmptyMMORPG.exe";
        process.StartInfo.Arguments = "-batchmode -nographics -mserver";
        process.Start();
    }

    static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
    {
        //* Do your stuff with the output (write to console/log/StringBuilder)
        UnityEngine.Debug.Log(outLine.Data);
    }
}
