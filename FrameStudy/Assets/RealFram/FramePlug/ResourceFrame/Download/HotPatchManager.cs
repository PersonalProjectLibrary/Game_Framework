using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class HotPatchManager : Singleton<HotPatchManager>//继承单例类
{
    /// <summary>
    /// 当前版本号
    /// </summary>
    private string m_CurVersion;
    /// <summary>
    /// 当前热更包名
    /// </summary>
    private string m_CurPackName;

    /// <summary>
    /// 用于开启协程
    /// </summary>
    private MonoBehaviour m_Mono;
    /// <summary>
    /// 服务器配置表下载后存储位置
    /// </summary>
    private string m_ServerXmlPath = Application.persistentDataPath + "/ServerInfo.xml";
    /// <summary>
    /// 存储xml反序列后的结果
    /// </summary>
    private ServerInfo m_ServerInfo;

    /// <summary>
    /// 当前游戏版本
    /// </summary>
    private VersionInfo m_GameVersion;
    /// <summary>
    /// 所有需要热更的东西，每次热更前记得清空下
    /// </summary>
    private Dictionary<string,Patch> m_HotFixDic = new Dictionary<string,Patch>();

    //GameStart里来调用这个方法进行初始化
    public void Init(MonoBehaviour mono)    //之前分离过程序集，用这种方式使用MonoBehaviour，和外界避开
    {
        m_Mono = mono;
    }

    /// <summary>
    /// 下载配置表，检查是否有热更
    /// </summary>
    /// 下载服务器列表要用协程，所以这里用到了回调，协程要用到MonoBehaviour类
    /// 读取本地版本后，读取服务器的xml，即下载服务器的xml
    /// <param name="hotCallBack">外部回调，默认为空，用于告诉是否有回调，UI界面该做如何的显示</param>
    public void CheckVersion(Action<bool> hotCallBack = null)
    {
        m_HotFixDic.Clear();
        ReadVersion();
        m_Mono.StartCoroutine(ReadXml(() =>
        {
            if(m_ServerInfo == null)
            {
                //这里临时处理，后续完善
                if(hotCallBack != null)hotCallBack(false);
                return;
            }
            //读取所有游戏版本，找当前版本
            foreach(VersionInfo version in m_ServerInfo.GameVersion)
            {
                if(version.NowVersion == m_CurVersion)
                {
                    m_GameVersion = version;
                    break;
                }
            }
            //获取热更的ab包
            GetHotAB();

            //这里没有进行文件对比，后续添加，这里临时代码做获取所有热更包后的处理
            if(hotCallBack!=null)hotCallBack(m_HotFixDic.Count>0);

        }));
    }

    /// <summary>
    /// 读当前本地版本信息
    /// 读Assets/Resources下的Version.txt文件
    /// </summary>
    void ReadVersion()
    {
        TextAsset versionText = Resources.Load<TextAsset>("Version");
        if (versionText == null)
        {
            Debug.LogError("未读到本地版本！");
            return;
        }
        string[] all = versionText.text.Split('\n');
        if (all.Length > 0)
        {
            string[] infoList = all[0].Split(';');
            if (infoList.Length >= 2) {
                m_CurVersion = infoList[0].Split('|')[1];
                m_CurPackName = infoList[1].Split('|')[1];
            }
        }
    }

    /// <summary>
    /// 获取服务器版本信息
    /// 协程下载服务器配置表
    /// </summary>
    /// <param name="callBack"></param>
    /// <returns></returns>
    IEnumerator ReadXml(Action callBack)
    {
        string xmlUrl = "http://127.0.0.1/ServerInfo.xml";//服务器上配置表地址
        UnityWebRequest webRequest = UnityWebRequest.Get(xmlUrl);
        webRequest.timeout = 30;//设置30秒超时时间
        yield return webRequest.SendWebRequest();//等待请求结束，下载结束
        if (webRequest.result == UnityWebRequest.Result.ConnectionError) 
            Debug.Log("Download Error" + webRequest.error);//超时后会进入
        else
        {
            //把下载的数据写成文件
            FileTool.CreateFile(m_ServerXmlPath,webRequest.downloadHandler.data);
            if (File.Exists(m_ServerXmlPath))
                m_ServerInfo = BinarySerializeOpt.XmlDeserialize(m_ServerXmlPath, typeof(ServerInfo)) as ServerInfo;
            else Debug.LogError("热更配置读取错误！");
        }
        if(callBack != null)callBack();
    }

    /// <summary>
    /// 获取所有热更包信息
    /// </summary>
    void GetHotAB()
    {
        //当前游戏版本不为空，热更包不为空，且热更包里有热更内容
        if (m_GameVersion != null && m_GameVersion.Patchs != null && m_GameVersion.Patchs.Length > 0)
        {
            //获取这个版本里的最后一次热更，累积热更比较麻烦，这里所有的热更都是基于当前初始版本做的热更
            Patchs lastPatches = m_GameVersion.Patchs[m_GameVersion.Patchs.Length - 1];
            //热更不能为空，热更包也不能为空
            if(lastPatches != null && lastPatches.Files != null)
            {
                //获取所有热更的热更包，这里简单获取，没有做热更一半，文件夹里东西被改变了等情况的处理，后面再做处理
                foreach (Patch patch in lastPatches.Files)
                {
                    m_HotFixDic.Add(patch.Name, patch);
                }
            }
        }
    }
}

public class FileTool
{
    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="bytes">写入的字节流</param>
    public static void CreateFile(string filePath, byte[] bytes)
    {
        if(File.Exists(filePath)) File.Delete(filePath);
        FileInfo file = new FileInfo(filePath);
        Stream stream = file.Create();
        stream.Write(bytes, 0, bytes.Length);
        stream.Close();
        stream.Dispose();
    }
}
