using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    private MonoBehaviour m_Mono;// 用于开启协程
    //服务器配置表下载后存储位置
    private string m_ServerXmlPath = Application.persistentDataPath + "/ServerInfo.xml";
    private ServerInfo m_ServerInfo;//存储服务器配置表xml反序列后的结果

    //本地以往的服务器配置表存储位置
    private string m_LocalXmlPath = Application.persistentDataPath + "/LocalInfo.xml";
    private ServerInfo m_LocalInfo;//存储本地配置表xml反序列化后的数据

    private VersionInfo m_GameVersion;// 当前游戏版本
    private Patchs m_CurrentPatches;//当前热更Patchs

    // 热更的所有东西，每次热更前记得清空下
    private Dictionary<string,Patch> m_HotFixDic = new Dictionary<string,Patch>();
    // 所有需要下载的东西
    private List<Patch> m_DownLoadList = new List<Patch>();
    // 所有需要下载的东西的dic
    private Dictionary<string,Patch> m_DownLoadDic = new Dictionary<string,Patch>();
    //下载的资源的保存位置
    private string m_DownloadPath = Application.persistentDataPath + "/DownLoad";

    //方便外面计算：加载速度、当前下载了多少，进度条等
    /// <summary>
    /// 需要下载的资源总个数，默认0
    /// </summary>
    public int LoadFileCount { get; set; } = 0;
    /// <summary>
    /// 需要下载资源的总大小 单位KB，默认0
    /// </summary>
    public float LoadSumSize { get; set; } = 0;

    //GameStart里来调用这个方法进行初始化，之前分离过程序集，用这种方式使用MonoBehaviour，和外界避开
    public void Init(MonoBehaviour mono)
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
            GetHotAB();//获取服务器上所有可能需要的热更资源

            //判断是否需要热更
            if(CheckLocalAndServerPatch())
            {
                ComputeDownload();//计算要下载的热更包
                if (File.Exists(m_ServerXmlPath))
                {
                    if(File.Exists(m_LocalXmlPath))File.Delete(m_LocalXmlPath);
                    File.Move(m_ServerXmlPath, m_LocalXmlPath);//将服务器热更xml名改换为本地热更xml名
                }
            }
            else ComputeDownload();//服务器信息与本地信息完全一致，检查本地patch资源与服务器patch资源是否一致

            //计算资源大小
            LoadFileCount = m_DownLoadList.Count;
            LoadSumSize = m_DownLoadList.Sum(x=>x.Size);

            //这里没有进行文件对比，后续添加，这里临时代码做获取所有热更包后的处理
            //这次改为计算下载资源，而不是所有热更资源，这里将m_HotFixDic.Count>0改为m_DownLoadList.Count>0
            if (hotCallBack!=null)hotCallBack(m_DownLoadList.Count>0);

        }));
    }

    /// <summary>
    /// 本地配置配置表与服务器热更配置表比较，是否需要更新
    /// </summary>
    /// <returns>true：需要热更；false：不需要热更</returns>
    bool CheckLocalAndServerPatch()
    {
        //本地不存在配置表，即首次进行热更
        if (!File.Exists(m_LocalXmlPath)) return true;

        //本地有服务器配置表，反序列化本地配置表
        m_LocalInfo = BinarySerializeOpt.XmlDeserialize(m_LocalXmlPath,typeof(ServerInfo)) as ServerInfo;

        VersionInfo localGameVersion = null;
        if (m_LocalInfo != null)
        {
            foreach(VersionInfo version in m_LocalInfo.GameVersion)
            {
                if (version.NowVersion == m_CurVersion)//配置表里当前这个的版号就是当前游戏版号
                {
                    localGameVersion = version;
                    break;
                }
            }
        }
        //两个服务器版本不同
        if (m_GameVersion.Patchs != null && localGameVersion != null &&  localGameVersion.Patchs != null && m_GameVersion.Patchs.Length > 0 && m_GameVersion.Patchs[m_GameVersion.Patchs.Length - 1].PatchVersion != localGameVersion.Patchs[localGameVersion.Patchs.Length - 1].PatchVersion)
            return true;

        return false;
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
    /// 获取服务器上所有可能需要的热更资源
    /// 后续计算ab包会用到
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

    /// <summary>
    /// 计算要下载的资源
    /// 也是：
    /// 检查本地资源是否与服务器下载列表信息一致，
    /// 主要用于在下载一半退出，再进入游戏，下载剩下部分
    /// 虽然未达到断点续传，但达到了已下载、未下载的分类
    /// </summary>
    void ComputeDownload()
    {
        m_DownLoadList.Clear();
        m_DownLoadDic.Clear();
        if (m_GameVersion != null && m_GameVersion.Patchs != null && m_GameVersion.Patchs.Length > 0)
        {
            m_CurrentPatches = m_GameVersion.Patchs[m_GameVersion.Patchs.Length - 1];
            if(m_CurrentPatches.Files!= null && m_CurrentPatches.Files.Count > 0)
            {
                foreach (Patch patch in m_CurrentPatches.Files)
                {
                    //不同平台不一样
                    if ((Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) && patch.Platform.Contains("StandaloneWindows64"))
                        AddDownloadList(patch);
                    else if ((Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor) && patch.Platform.Contains("Android"))
                        AddDownloadList(patch);
                    else if ((Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WindowsEditor) && patch.Platform.Contains("IOS"))
                        AddDownloadList(patch);
                }
            }
        }
    }

    //把要下载的资源加入下载队列：m_DownLoadList/m_DownLoadDic
    void AddDownloadList(Patch patch)
    {
        string filePath = m_DownloadPath + "/" + patch.Name;
        //下载下的文件与本地文件对比，对比MD5码，看本地文件是否有被修改
        if (File.Exists(filePath))
        {
            string md5 = MD5Manager.Instance.BuildFileMd5(filePath);
            if (patch.Md5 != md5)
            {
                //本地被修改过，重新下载
                m_DownLoadList.Add(patch);
                m_DownLoadDic.Add(patch.Name, patch);
            }
        }
        else
        {
            //本地不存在，直接下载获取
            m_DownLoadList.Add(patch);
            m_DownLoadDic.Add(patch.Name, patch);
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
