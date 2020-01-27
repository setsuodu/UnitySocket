using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;

public class Proto2CSEditor : EditorWindow
{
	[MenuItem("Tools/Proto2CS")]
	public static void AllProto2CS()
	{
        ClearConsole();

        string root = Path.GetFullPath(@".."); //上级目录
        //string dir2 = Path.GetFullPath(@"..//.."); //上上级目录
        string protoRoot = "Protoc/bin";
        string outputDir = "cs";
        string fileName = "run.bat";

        string batFilePath = Path.Combine(root, protoRoot, fileName);
        //UnityEngine.Debug.Log(batFilePath);
        RunBat(batFilePath);

        string outputPath = Path.Combine(root, protoRoot, outputDir);
        if (!Directory.Exists(outputPath)) 
            Directory.CreateDirectory(outputPath);
        //UnityEngine.Debug.Log(outputPath);
        OpenExplorer(outputPath);

        // 遍历目录下所有文件
        DirectoryInfo outputInfo = new DirectoryInfo(outputPath);
        FileInfo[] files = outputInfo.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            string _src = files[i].FullName;

            string _client = Path.Combine(Application.dataPath, "Scripts/Model", files[i].Name);
            UnityEngine.Debug.LogFormat("{0}\n<color=green>[==>client]</color> {1}", _src, _client);
            CopyTo(_src, _client);

            string serverModelDir = @"Server\SocketServerTCP\Model";
            string _server = Path.Combine(root, serverModelDir, files[i].Name);
            UnityEngine.Debug.LogFormat("{0}\n<color=green>[==>server]</color> {1}", _src, _server);
            CopyTo(_src, _server);
        }
    }

    static void OpenExplorer(string filePath)
    {
        //Process.Start("c:\\");
        Process.Start(filePath);
    }

    static void OpenIE(string url)
    {
        Process ie = new Process();
        ie.StartInfo.FileName = "IEXPLORE.EXE";
        ie.StartInfo.Arguments = @"http://www.baidu.com";
        ie.Start();
    }

    static void RunBat(string filePath)
    {
        try
        {
            Process bat = new Process();
            bat.StartInfo.FileName = filePath;
            bat.Start();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.Message);
        }
    }

    static void CopyTo(string srcPath, string dstPath) 
    {
        if (!File.Exists(srcPath)) 
        {
            UnityEngine.Debug.LogError("源文件不存在：" + srcPath);
            return;
        }
        if (File.Exists(dstPath)) 
        {
            File.Delete(dstPath);
            UnityEngine.Debug.Log("<color=red>删除旧文件：</color>" + dstPath);
        }
        File.Copy(srcPath, dstPath);
    }

    static void ClearConsole()
    {
        Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
        Type logEntries = assembly.GetType("UnityEditor.LogEntries");
        MethodInfo clearConsoleMethod = logEntries.GetMethod("Clear");
        clearConsoleMethod.Invoke(new object(), null);

        //UnityEngine.Debug.Log("<color=green>clear!</color>");
    }
}
