using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Profiling;
using NUnit.Framework;

public class BundleEditor
{
    /// <summary>
    /// 打的包生成的地址
    /// </summary>
    /// 不同平台渠道：
    /// EditorUserBuildSettings.activeBuildTarget.ToString()
    private static string m_BunleTargetPath = Application.dataPath+"/../AssetBundle/" + EditorUserBuildSettings.activeBuildTarget.ToString();
    /// <summary>
    /// 版本文件所在路径
    /// </summary>
    private static string m_VersionMd5Path = Application.dataPath+"/../Version/"+EditorUserBuildSettings.activeBuildTarget.ToString();
    /// <summary>
    /// 热更的路径
    /// </summary>
    /// 热更相关的文件夹，存储有更新的资源
    private static string m_HotPath = Application.dataPath + "/../Hot/" + EditorUserBuildSettings.activeBuildTarget.ToString();
    /// <summary>
    /// ab包的配置路径
    /// </summary>
    private static string ABCONFIGPATH = "Assets/RealFram/Editor/Resource/ABConfig.asset";
    /// <summary>
    /// ab包的字节路径
    /// </summary>
    private static string ABBYTEPATH = RealConfig.GetRealFram().m_ABBytePath;
    /// <summary>
    /// 记录所有ab包的文件夹的dic
    /// [ab包名，路径]
    /// </summary>
    private static Dictionary<string, string> m_AllFileDir = new Dictionary<string, string>();
    /// <summary>
    /// AB包的所有文件路径
    /// </summary>
    /// 过滤的list
    private static List<string> m_AllFileAB = new List<string>();
    /// <summary>
    /// 单个prefab的ab包
    /// [prefab名，prefab的依赖资源列表]
    /// </summary>
    private static Dictionary<string, List<string>> m_AllPrefabDir = new Dictionary<string, List<string>>();
    /// <summary>
    /// 储存所有配置文件的有效路径
    /// </summary>
    /// 包括AB包路径、预制体路径
    private static List<string> m_ConfigFile = new List<string>();

    /// <summary>
    /// 存储本地存在的MD5
    /// </summary>
    private static Dictionary<string,ABMD5Base> m_PackedMd5 = new Dictionary<string,ABMD5Base>();

    [MenuItem("Tools/测试重复加密")]
    public static void TestAESEnc()
    {
        //filePath带文件名和后缀，设置的密钥
        AES.AESFileEncrypt(Application.dataPath + "/GameData/Data/Xml/AESFileEncrptyData.xml", "Ocean");
    }

    [MenuItem("Tools/非热更打包，更新MD5")]
    public static void NormalBuild()
    {
        Build();//标准打包，非热更打包
    }

    /// <summary>
    /// 打包
    /// 参数不加，是非热更打包
    /// 使用参数，是进行热更打包
    /// </summary>
    /// <param name="hotFix">是否热更打包</param>
    /// <param name="abmd5Path">热更的md5版本信息路径</param>
    /// <param name="hotCount">热更次数</param>
    public static void Build(bool hotFix = false, string abmd5Path = "", string hotCount = "1")
    {
        DataEditor.AllXmlToBinary();
        m_ConfigFile.Clear();
        m_AllFileAB.Clear();
        m_AllFileDir.Clear();
        m_AllPrefabDir.Clear();
        ABConfig abConfig = AssetDatabase.LoadAssetAtPath<ABConfig>(ABCONFIGPATH);
        foreach (ABConfig.FileDirABName fileDir in abConfig.m_AllFileDirAB)
        {
            if (m_AllFileDir.ContainsKey(fileDir.ABName))
            {
                Debug.LogError("AB包配置名字重复，请检查！");
            }
            else
            {
                m_AllFileDir.Add(fileDir.ABName, fileDir.Path);
                m_AllFileAB.Add(fileDir.Path);
                m_ConfigFile.Add(fileDir.Path);
            }
        }

        string[] allStr = AssetDatabase.FindAssets("t:Prefab", abConfig.m_AllPrefabPath.ToArray());
        for (int i = 0; i < allStr.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(allStr[i]);
            EditorUtility.DisplayProgressBar("查找Prefab", "Prefab:" + path, i * 1.0f / allStr.Length);
            m_ConfigFile.Add(path);
            if (!ContainAllFileAB(path))
            {
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                string[] allDepend = AssetDatabase.GetDependencies(path);
                List<string> allDependPath = new List<string>();
                for (int j = 0; j < allDepend.Length; j++)
                {
                    if (!ContainAllFileAB(allDepend[j]) && !allDepend[j].EndsWith(".cs"))
                    {
                        m_AllFileAB.Add(allDepend[j]);
                        allDependPath.Add(allDepend[j]);
                    }
                }
                if (m_AllPrefabDir.ContainsKey(obj.name)) Debug.LogError("存在相同名字的Prefab！名字：" + obj.name);
                else m_AllPrefabDir.Add(obj.name, allDependPath);
            }
        }

        foreach (string name in m_AllFileDir.Keys) SetABName(name, m_AllFileDir[name]);

        foreach (string name in m_AllPrefabDir.Keys) SetABName(name, m_AllPrefabDir[name]);

        //打包AB资源包
        BunildAssetBundle();
        //清除旧AB资源包
        string[] oldABNames = AssetDatabase.GetAllAssetBundleNames();
        for (int i = 0; i < oldABNames.Length; i++)
        {
            AssetDatabase.RemoveAssetBundleName(oldABNames[i], true);
            EditorUtility.DisplayProgressBar("清除AB包名", "名字：" + oldABNames[i], i * 1.0f / oldABNames.Length);
        }
        
        if (hotFix) ReadMd5Com(abmd5Path, hotCount);//筛选有改变/热更的资源，并复制到热更文件夹下
        else WriteABMD5();//正常写入ab包资源信息

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
    }

    /// <summary>
    /// 写入资源信息
    /// </summary>
    static void WriteABMD5()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(m_BunleTargetPath);//从资源文件夹里获取文件信息
        FileInfo[] files = directoryInfo.GetFiles("*",SearchOption.AllDirectories);//获取所有文件信息。*表示所有文件信息
        //文件写入
        ABMD5 abmd5 = new ABMD5();
        abmd5.ABMD5List = new List<ABMD5Base>();
       for(int i=0;i< files.Length; i++)
        {
            //不是mate文件、manifest文件，则可进行写入
            if (!files[i].Name.EndsWith(".meta") && !files[i].Name.EndsWith(".manifest"))
            {
                ABMD5Base abmd5Base = new ABMD5Base();
                abmd5Base.Name = files[i].Name;
                abmd5Base.Md5 = MD5Manager.Instance.BuildFileMd5(files[i].FullName);//传入全路径
                abmd5Base.Size = files[i].Length / 1024.0f;//单位kb
                abmd5.ABMD5List.Add(abmd5Base);
            }
        }
        //储存md5
        string ABMD5Path = Application.dataPath + "/Resources/ABMD5.bytes";

        //序列化二进制
        BinarySerializeOpt.BinarySerilize(ABMD5Path, abmd5);

        //将打版的版本文件MD5，拷贝到外部（m_VersionMd5Path）进行储存
        if (!Directory.Exists(m_VersionMd5Path)) Directory.CreateDirectory(m_VersionMd5Path);
        //文件夹+文件名+版本号+后缀
        string targetPath = m_VersionMd5Path + "/ABMD5_" + PlayerSettings.bundleVersion + ".bytes";
        if(File.Exists(targetPath))File.Delete(targetPath);
        File.Copy(ABMD5Path, targetPath);
    }

    /// <summary>
    /// 筛选、拷贝热更后/有改变的资源
    /// </summary>
    /// <param name="abmd5Path"></param>
    /// <param name="hotCount"></param>
    /// 1、读取以往MD5文件；2、对当前ab资源生成新MD5；
    /// 3、比较MD5：MD5有变化的，对应文件资源有更新修改。
    /// 4、记录热更的资源，并生成新的配置表
    static void ReadMd5Com(string abmd5Path,string hotCount)
    {
        m_PackedMd5.Clear();
        //读取记录文件夹中已存储的md5
        using(FileStream fileStream = new FileStream(abmd5Path, FileMode.Open, FileAccess.Read))
        {
            BinaryFormatter bf = new BinaryFormatter();
            ABMD5 abmd5 = bf.Deserialize(fileStream) as ABMD5;
            foreach(ABMD5Base abmd5Base in abmd5.ABMD5List)
            {
                m_PackedMd5.Add(abmd5Base.Name, abmd5Base);
            }
        }
        
        List<string> changeList = new List<string>();//记录已改变的资源
        DirectoryInfo directoryInfo = new DirectoryInfo(m_BunleTargetPath);//获取文件夹信息
        FileInfo[] files = directoryInfo.GetFiles("*", SearchOption.AllDirectories);//获取文件信息
        for(int i = 0;i< files.Length; i++)
        {
            if (!files[i].Name.EndsWith(".meta") && !files[i].Name.EndsWith(".manifest"))
            {
                string name = files[i].Name;
                string md5 = MD5Manager.Instance.BuildFileMd5(files[i].FullName);//本次根据ab包的资源信息，新打包的MD5文件
                ABMD5Base abmd5Base = null;//存储本地同名的md5
                if (!m_PackedMd5.ContainsKey(name)) changeList.Add(name);//已经打包的MD5文件夹中不含有此版本的MD5，此次是新版本打包的MD5
                else
                {
                    if(m_PackedMd5.TryGetValue(name,out abmd5Base))//在过去打包的MD5中找到同版本号的MD5文件
                    {
                        if(md5!=abmd5Base.Md5)changeList.Add(name);//旧版本的MD5和这次打包的MD5数据不一样，说明资源更新了
                    }
                }
            }
        }
        CopyABAndGenerateXML(changeList, hotCount);
    }

    /// <summary>
    /// 拷贝改变的AB包资源，生成服务器配置表
    /// </summary>
    /// <param name="changeList">已改变的资源列表</param>
    /// <param name="hotCount">热更次数</param>
    static void CopyABAndGenerateXML(List<string> changeList,string hotCount)
    {
        //热更资源文件夹不存在，则生成热更资源文件夹
        if(!Directory.Exists(m_HotPath))Directory.CreateDirectory(m_HotPath);
        //删除文件夹里已有的AB包
        DeleteAllFile(m_HotPath);
        //拷贝这次改变的AB包
        foreach (string str in changeList)
        {
            if (!str.EndsWith(".manifest")) File.Copy(m_BunleTargetPath + "/" + str, m_HotPath + "/" + str);
        }
        //自动生成服务器配置表
        DirectoryInfo directory = new DirectoryInfo(m_HotPath);
        FileInfo[] files = directory.GetFiles("*",SearchOption.AllDirectories);
        //ServerInfo是总配置表，我们不从总配置表生成，只需每次从单个热更总包开始生成拷贝
        Patch patch = new Patch();
        patch.PatchVersion = 1;//这么默认1，可自行修改
        patch.PatchFiles = new List<PatchFile>();
        for (int i = 0; i < files.Length; i++)
        {
            PatchFile patchFile = new PatchFile();
            patchFile.Md5 = MD5Manager.Instance.BuildFileMd5(files[i].FullName);
            patchFile.Name = files[i].Name;
            patchFile.Size = files[i].Length / 1024.0f;
            patchFile.Platform = EditorUserBuildSettings.activeBuildTarget.ToString();
            //搭建本地服务器后，完善地址如下
            patchFile.Url = "http://127.0.0.1/AssetBundle/" + PlayerSettings.bundleVersion + "/" + hotCount + "/" + files[i].Name;
            patch.PatchFiles.Add(patchFile);
        }

        //序列化
        BinarySerializeOpt.Xmlserialize(m_HotPath+"/Patch.xml",patch);
    }

    static void SetABName(string name, string path)
    {
        AssetImporter assetImporter = AssetImporter.GetAtPath(path);
        if (assetImporter == null)
        {
            Debug.LogError("不存在此路径文件：" + path);
        }
        else
        {
            assetImporter.assetBundleName = name;
        }
    }

    static void SetABName(string name, List<string> paths)
    {
        for (int i = 0; i < paths.Count; i++)
        {
            SetABName(name, paths[i]);
        }
    }

    /// <summary>
    /// 打包AB资源包
    /// </summary>
    static void BunildAssetBundle()
    {
        string[] allBundles = AssetDatabase.GetAllAssetBundleNames();
        //key为全路径，value为包名
        Dictionary<string, string> resPathDic = new Dictionary<string, string>();
        for (int i = 0; i < allBundles.Length; i++)
        {
            string[] allBundlePath = AssetDatabase.GetAssetPathsFromAssetBundle(allBundles[i]);
            for (int j = 0; j < allBundlePath.Length; j++)
            {
                if (allBundlePath[j].EndsWith(".cs"))
                    continue;

                Debug.Log("此AB包：" + allBundles[i] + "下面包含的资源文件路径：" + allBundlePath[j]);
                resPathDic.Add(allBundlePath[j], allBundles[i]);
            }
        }

        if (!Directory.Exists(m_BunleTargetPath))
        {
            Directory.CreateDirectory(m_BunleTargetPath);
        }

        DeleteAB();
        //生成自己的配置表
        WriteData(resPathDic);

        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(m_BunleTargetPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        if (manifest == null)
        {
            Debug.LogError("AssetBundle 打包失败！");
        }
        else
        {
            Debug.Log("AssetBundle 打包完毕");
        }
    }

    static void WriteData(Dictionary<string ,string> resPathDic)
    {
        AssetBundleConfig config = new AssetBundleConfig();
        config.ABList = new List<ABBase>();
        foreach (string path in resPathDic.Keys)
        {
            if (!ValidPath(path))
                continue;

            ABBase abBase = new ABBase();
            abBase.Path = path;
            abBase.Crc = Crc32.GetCrc32(path);
            abBase.ABName = resPathDic[path];
            abBase.AssetName = path.Remove(0, path.LastIndexOf("/") + 1);
            abBase.ABDependce = new List<string>();
            string[] resDependce = AssetDatabase.GetDependencies(path);
            for (int i = 0; i < resDependce.Length; i++)
            {
                string tempPath = resDependce[i];
                if (tempPath == path || path.EndsWith(".cs"))
                    continue;

                string abName = "";
                if (resPathDic.TryGetValue(tempPath, out abName))
                {
                    if (abName == resPathDic[path])
                        continue;

                    if (!abBase.ABDependce.Contains(abName))
                    {
                        abBase.ABDependce.Add(abName);
                    }
                }
            }
            config.ABList.Add(abBase);
        }

        //写入xml
        string xmlPath = Application.dataPath + "/AssetbundleConfig.xml";
        if (File.Exists(xmlPath)) File.Delete(xmlPath);
        FileStream fileStream = new FileStream(xmlPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xs = new XmlSerializer(config.GetType());
        xs.Serialize(sw, config);
        sw.Close();
        fileStream.Close();

        //写入二进制
        foreach (ABBase abBase in config.ABList)
        {
            abBase.Path = "";
        }
        FileStream fs = new FileStream(ABBYTEPATH, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        fs.Seek(0, SeekOrigin.Begin);
        fs.SetLength(0);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, config);
        fs.Close();
        AssetDatabase.Refresh();
        SetABName("assetbundleconfig", ABBYTEPATH);
    }

    /// <summary>
    /// 删除无用AB包
    /// </summary>
    static void DeleteAB()
    {
        string[] allBundlesName = AssetDatabase.GetAllAssetBundleNames();
        DirectoryInfo direction = new DirectoryInfo(m_BunleTargetPath);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            if (ConatinABName(files[i].Name, allBundlesName) || files[i].Name.EndsWith(".meta")|| files[i].Name.EndsWith(".manifest") || files[i].Name.EndsWith("assetbundleconfig"))
            {
                continue;
            }
            else
            {
                Debug.Log("此AB包已经被删或者改名了：" + files[i].Name);
                if (File.Exists(files[i].FullName))
                {
                    File.Delete(files[i].FullName);
                }
                if(File.Exists(files[i].FullName + ".manifest"))
                {
                    File.Delete(files[i].FullName + ".manifest");
                }
            }
        }
    }

    /// <summary>
    /// 遍历文件夹检查判断所有的AB包
    /// </summary>
    /// <param name="name"></param>
    /// <param name="strs"></param>
    /// <returns></returns>
    static bool ConatinABName(string name, string[] strs)
    {
        for (int i = 0; i < strs.Length; i++)
        {
            if (name == strs[i])
                return true;
        }
        return false;
    }

    /// <summary>
    /// 是否包含在已有的AB包里
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    /// 判断当前ab包是否重复，用来做AB包冗余剔除
    static bool ContainAllFileAB(string path)
    {
        for (int i = 0; i < m_AllFileAB.Count; i++)
        {
            if (path == m_AllFileAB[i] || (path.Contains(m_AllFileAB[i]) && (path.Replace(m_AllFileAB[i],"")[0] == '/')))
                return true;
        }

        return false;
    }

    /// <summary>
    /// 是否是有效路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    static bool ValidPath(string path)
    {
        for (int i = 0; i < m_ConfigFile.Count; i++)
        {
            if (path.Contains(m_ConfigFile[i]))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 删除指定文件目录下的所有文件
    /// </summary>
    /// <param name="fullPath">全路径</param>
    /// <returns></returns>
    /// 非递归式的删除文件夹
    public static void DeleteAllFile(string fullPath)
    {
        if (Directory.Exists(fullPath))
        {
            DirectoryInfo directory = new DirectoryInfo(fullPath);
            FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta")) continue;
                File.Delete(files[i].FullName);
            }
        }
    }
}
