using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class ResourceTest : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/attack");
        //GameObject obj = GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("attack"));
        //GameObject obj = GameObject.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameData/Prefabs/Attack.prefab"));
        //BinaryDeSerTest();
        //ReadTestAssets();
        TestLoadAB();
    }

    /// <summary>
    /// 测试加载AB资源
    /// </summary>
    void TestLoadAB()
    {
        //AssetBundle configAB = AssetBundle.LoadFromFile (Application.streamingAssetsPath + "/assetbundleconfig");
        //TextAsset textAsset = configAB.LoadAsset<TextAsset>("AssetBundleConfig");
        //MemoryStream stream = new MemoryStream(textAsset.bytes);
        //BinaryFormatter bf = new BinaryFormatter();
        //AssetBundleConfig testSerilize = (AssetBundleConfig)bf.Deserialize(stream);
        //stream.Close();
        //string path = "Assets/GameData/Prefabs/Attack.prefab";
        //uint crc = Crc32.GetCrc32(path);
        //ABBase abBase = null;
        //for (int i = 0; i < testSerilize.ABList.Count; i++)
        //{
        //    if (testSerilize.ABList[i].Crc == crc)
        //    {
        //        abBase = testSerilize.ABList[i];
        //    }
        //}

        //for (int i = 0; i < abBase.ABDependce.Count; i++)
        //{
        //    AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + abBase.ABDependce[i]);
        //}
        //AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + abBase.ABName);
        //GameObject obj = GameObject.Instantiate(assetBundle.LoadAsset<GameObject>(abBase.AssetName));
    }

    /// <summary>
    /// 测试读取AB资源
    /// </summary>
    void ReadTestAssets()
    {
        //AssetsSerilize assets = UnityEditor.AssetDatabase.LoadAssetAtPath<AssetsSerilize>("Assets/Scripts/TestAssets.asset");
        //Debug.Log(assets.Id);
        //Debug.Log(assets.Name);
        //foreach (string str in assets.TestList)
        //{
        //    Debug.Log(str);
        //}
    }

    /// <summary>
    /// 测试xml序列化
    /// </summary>
    void SerilizeTest()
    {
        TestSerilize testSerilize = new TestSerilize();
        testSerilize.Id = 1;
        testSerilize.Name = "测试";
        testSerilize.List = new List<int>();
        testSerilize.List.Add(2);
        testSerilize.List.Add(3);
        XmlSerilize(testSerilize);
    }

    /// <summary>
    /// 测试xml反序列化
    /// </summary>
    void DeSerilizerTest()
    {
        TestSerilize testSerilize = XmlDeSerilize();
        Debug.Log(testSerilize.Id +"   " + testSerilize.Name);
        foreach (int a in testSerilize.List)
        {
            Debug.Log(a);
        }
    }

    /// <summary>
    /// xml序列化
    /// </summary>
    /// <param name="testSerilize"></param>
    void XmlSerilize(TestSerilize testSerilize)
    {
        FileStream fileStream = new FileStream(Application.dataPath + "/test.xml", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xml = new XmlSerializer(testSerilize.GetType());
        xml.Serialize(sw, testSerilize);
        sw.Close();
        fileStream.Close();
    }

    /// <summary>
    /// xml反序列化
    /// </summary>
    /// <returns></returns>
    TestSerilize XmlDeSerilize()
    {
        FileStream fs = new FileStream(Application.dataPath + "/test.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        XmlSerializer xs = new XmlSerializer(typeof(TestSerilize));
        TestSerilize testSerilize = (TestSerilize)xs.Deserialize(fs);
        fs.Close();
        return testSerilize;
    }

    /// <summary>
    /// 测试二进制序列化
    /// </summary>
    void BinarySerTest()
    {
        TestSerilize testSerilize = new TestSerilize();
        testSerilize.Id = 5;
        testSerilize.Name = "二进制测试";
        testSerilize.List = new List<int>();
        testSerilize.List.Add(10);
        testSerilize.List.Add(18);
        BinarySerilize(testSerilize);
    }

    //测试二进制反序列化
    //void BinaryDeSerTest()
    //{
    //    TestSerilize testSerilize = BinaryDeserilize();
    //    Debug.Log(testSerilize.Id + "   " + testSerilize.Name);
    //    foreach (int a in testSerilize.List)
    //    {
    //        Debug.Log(a);
    //    }
    //}

    /// <summary>
    /// 二进制序列化
    /// </summary>
    /// <param name="serilize"></param>
    void BinarySerilize(TestSerilize serilize)
    {
        FileStream fs = new FileStream(Application.dataPath + "/test.bytes", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, serilize);
        fs.Close();
    }

    //二进制反序列化
    //TestSerilize BinaryDeserilize()
    //{
    //    TextAsset textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/test.bytes");
    //    MemoryStream stream = new MemoryStream(textAsset.bytes);
    //    BinaryFormatter bf = new BinaryFormatter();
    //    TestSerilize testSerilize = (TestSerilize)bf.Deserialize(stream);
    //    stream.Close();
    //    return testSerilize;
    //}
}
