using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class TestEditor
{
    [MenuItem("TTT/JenkinsTest")]
    public static void JenkinsTest()
    {
        FileInfo fileInfo = new FileInfo(Application.dataPath + "/测试.txt");
        StreamWriter sw = fileInfo.CreateText();
        sw.WriteLine(System.DateTime.Now);
        sw.Close();
        sw.Dispose();
    }

    private static Sprite ttt;

    [MenuItem("测试/测试加载")]
    public static void TestLoad()
    {
        ttt = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GameData/UGUI/Test1.png");
    }

    [MenuItem("测试/测试卸载")]
    public static void TestUnLoad()
    {
        Resources.UnloadAsset(ttt);//对引用进行了释放，但是还存在在编辑器内存
    }

    private static string DLLPATH = "Assets/GameData/Data/HotFix/HotFix.dll";
    private static string PDBPATH = "Assets/GameData/Data/HotFix/HotFix.pdb";

    [MenuItem("Tools/修改热更dll为bytes")]
    public static void ChangeDllName()
    {
        if(File.Exists(DLLPATH)) File.Move(DLLPATH, DLLPATH+ ".bytes");
        if (File.Exists(PDBPATH)) File.Move(PDBPATH, PDBPATH + ".bytes");
        AssetDatabase.Refresh();//刷新
    }
}
