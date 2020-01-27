using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Path = System.IO.Path;
using UnityEngine;
using UnityEditor;

internal class OpcodeInfo
{
	public string Name;
	public int Opcode;
}

public class Proto2CSEditor : EditorWindow
{
	[MenuItem("Tools/Proto2CS")]
	public static void AllProto2CS()
	{
		Process process = ProcessHelper.Run("dotnet", "Proto2CS.dll", "../Proto/", true);
		UnityEngine.Debug.Log(process.StandardOutput.ReadToEnd());
		AssetDatabase.Refresh();
	}
}

public static class ProcessHelper
{
    public static Process Run(string exe, string arguments, string workingDirectory = ".", bool waitExit = false)
    {
        try
        {
            bool redirectStandardOutput = true;
            bool redirectStandardError = true;
            bool useShellExecute = false;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                redirectStandardOutput = false;
                redirectStandardError = false;
                useShellExecute = true;
            }

            if (waitExit)
            {
                redirectStandardOutput = true;
                redirectStandardError = true;
                useShellExecute = false;
            }

            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = exe,
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = useShellExecute,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = redirectStandardOutput,
                RedirectStandardError = redirectStandardError,
            };

            Process process = Process.Start(info);

            if (waitExit)
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception($"{process.StandardOutput.ReadToEnd()} {process.StandardError.ReadToEnd()}");
                }
            }

            return process;
        }
        catch (Exception e)
        {
            throw new Exception($"dir: {Path.GetFullPath(workingDirectory)}, command: {exe} {arguments}", e);
        }
    }
}