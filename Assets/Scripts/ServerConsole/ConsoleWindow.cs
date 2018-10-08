using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Windows
{
    public class ConsoleWindow
    {
        TextWriter oldOutput;

        public void Initialize()
        {
			if (!AttachConsole(0x0ffffffff))
			{
				AllocConsole();
			}

			try
			{
				IntPtr stdHandle = GetStdHandle( STD_OUTPUT_HANDLE );
				Microsoft.Win32.SafeHandles.SafeFileHandle safeFileHandle = new Microsoft.Win32.SafeHandles.SafeFileHandle( stdHandle, true );
				FileStream fileStream = new FileStream( safeFileHandle, FileAccess.Write );
				System.Text.Encoding encoding = System.Text.Encoding.ASCII;
				StreamWriter standardOutput = new StreamWriter( fileStream, encoding );
				standardOutput.AutoFlush = true;
				Console.SetOut( standardOutput );
			}
			catch (System.Exception e)
			{
				Debug.Log("Could not redirect output: " + e.Message);
			}
        }

        public void Shutdown()
        {
            Console.SetOut(oldOutput);
        }

        public void SetTitle(string name)
        {
            SetConsoleTitle(name);
        }

        private const int STD_OUTPUT_HANDLE = -11;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleTitle(string lpConsoleTitle);
    }

}