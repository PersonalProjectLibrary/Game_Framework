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

    /// <summary>
    /// 记录了GameStart的MonoBehaviour，用此变量方便开启协程
    /// Init()里设置了，StartDownLoadAB()里使用了
    /// </summary>
    private MonoBehaviour m_Mono;
    /// <summary>
    /// 服务器配置表下载后存储位置
    /// </summary>
    private string m_ServerXmlPath = Application.persistentDataPath + "/ServerInfo.xml";
    /// <summary>
    /// 存储服务器配置表xml反序列后的结果
    /// </summary>
    private ServerInfo m_ServerInfo;

    /// <summary>
    /// 本地以往的服务器配置表存储位置
    /// </summary>
    private string m_LocalXmlPath = Application.persistentDataPath + "/LocalInfo.xml";
    /// <summary>
    /// 存储本地配置表xml反序列化后的数据
    /// </summary>
    private ServerInfo m_LocalInfo;

    /// <summary>
    /// 当前游戏版本
    /// </summary>
    private VersionInfo m_GameVersion;
    /// <summary>
    /// 当前热更Patchs
    /// </summary>
    private Patchs m_CurrentPatches;

    /// <summary>
    /// 热更的所有东西，每次热更前记得清空下
    /// </summary>
    private Dictionary<string,Patch> m_HotFixDic = new Dictionary<string,Patch>();
    /// <summary>
    /// 所有需要下载的东西
    /// </summary>
    private List<Patch> m_DownLoadList = new List<Patch>();
    /// <summary>
    /// 所有需要下载的东西的dic
    /// </summary>
    private Dictionary<string,Patch> m_DownLoadDic = new Dictionary<string,Patch>();
    /// <summary>
    /// 下载的资源的保存位置
    /// </summary>
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

    /// <summary>
    /// 储存已经下载的资源
    /// </summary>
    public List<Patch> m_AlreadyDownList = new List<Patch>();
    /// <summary>
    /// 是否开始下载资源
    /// 开始下载时进行开启，MD5码校验后关闭
    /// </summary>
    private bool m_StartDownload = false;
    /// <summary>
    /// 服务器上的资源名对应的MD5,用于下载后MD5校验，在设置m_DownLoadList时一起设置
    /// </summary>
    private Dictionary<string,string> m_DownLoadMD5Dic = new Dictionary<string,string>();
    /// <summary>
    /// 服务器列表获取错误回调
    /// </summary>
    public Action ServerInfoError;//视频021里还没有添加，后面也没有见到添加该回调，到视频026开头可发现那里已添加这个回调
    /// <summary>
    /// 重复下载次数
    /// </summary>
    private int m_TryDownCount = 0;
    /// <summary>
    /// 最多重复下载几次
    /// </summary>
    private const int DOWNLOADCOUNT = 4;
    /// <summary>
    /// 重复下载失败回调
    /// </summary>
    public Action<string> ItemError;
    /// <summary>
    /// 下载完成回调
    /// </summary>
    public Action LoadOver;

    //GameStart里来调用这个方法进行初始化，之前分离过程序集，用这种方式使用MonoBehaviour，和外界避开
    /// <summary>
    /// 使用调用Init方法的脚本的MonoBehaviour，
    /// 使用MonoBehaviour的协程方法
    /// </summary>
    /// <param name="mono"></param>
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
        m_TryDownCount = 0;
        m_HotFixDic.Clear();
        ReadVersion();
        m_Mono.StartCoroutine(ReadXml(() =>
        {
            if(m_ServerInfo == null)
            {
                if(ServerInfoError != null) ServerInfoError();
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
            //视频022尾部里添加但没写完，023视频里都没有继续完善（没剪出来），后面变成直接使用ComputeDownload();
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
    /// 计算要下载的资源，并把要下载的资源加入下载队列
    /// 也是检查本地资源是否与服务器下载列表信息一致
    /// </summary>
    void ComputeDownload()
    {
        m_DownLoadList.Clear();
        m_DownLoadDic.Clear();
        m_DownLoadMD5Dic.Clear();

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

    /// <summary>
    /// 把要下载的资源加入下载队列：m_DownLoadList/m_DownLoadDic
    /// </summary>
    /// <param name="patch"></param>
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
                m_DownLoadMD5Dic.Add(patch.Name,patch.Md5);
            }
        }
        else
        {
            //本地不存在，直接下载获取
            m_DownLoadList.Add(patch);
            m_DownLoadDic.Add(patch.Name, patch);
            m_DownLoadMD5Dic.Add(patch.Name, patch.Md5);
        }
    }

    /// <summary>
    /// 开始下载AB包
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public IEnumerator StartDownLoadAB(Action callback,List<Patch> allPatch = null)
    {
        m_AlreadyDownList.Clear();
        m_StartDownload = true;
        if(!Directory.Exists(m_DownloadPath)) Directory.CreateDirectory(m_DownloadPath);
        //要下载的资源信息Patch，记录在m_DownLoadList里，
        //根据m_DownLoadList，把Patch解析成DownLoadAssetBundle格式存储
        List<DownLoadAssetBundle> downLoadAssetBundles = new List<DownLoadAssetBundle>();
        //由于重复下载下载失败的资源使用到该协程，这里原本只有callback参数，添加allPatch
        //添加判断，不传参时，默认是m_DownLoadList，传参的是要重复下载的资源列表
        if (allPatch == null) allPatch = m_DownLoadList;
        foreach (Patch patch in allPatch)
        {
            downLoadAssetBundles.Add(new DownLoadAssetBundle(patch.Url, m_DownloadPath));
        }
        //根据DownLoadAssetBundle，下载AB资源
        foreach (DownLoadAssetBundle downLoadAB in downLoadAssetBundles)
        {
            yield return m_Mono.StartCoroutine(downLoadAB.Download());
            Patch patch = FindPatchByName(downLoadAB.FileName);
            if(patch != null) m_AlreadyDownList.Add(patch);
            downLoadAB.DestoryDownload();
        }

        //文件全部下载后，对所有文件进行MD5码的校验，自动重新下载校验没通过的文件
        //并对下载次数进行重复下载计数，达到一定次数后，反馈某文件下载失败
        VerifyMD5(downLoadAssetBundles, callback);
    }

    /// <summary>
    /// 根据名字查找对象的热更patch
    /// 通过m_DownLoadDic来查找
    /// </summary>
    /// <param name="name">Patch名</param>
    /// <returns>返回热更patch</returns>
    Patch FindPatchByName(string name)
    {
        Patch patch = null;
        m_DownLoadDic.TryGetValue(name, out patch);
        return patch;
    }

    /// <summary>
    /// 校验下载的文件MD5码,与储存在字典里的md5码是否一致
    /// </summary>
    /// <param name="downLoadAssets"></param>
    /// <param name="callBack"></param>
    void VerifyMD5(List<DownLoadAssetBundle> downLoadAssets, Action callBack)
    {
        List<Patch> downLoadList = new List<Patch>();//重新下载列表
        foreach (DownLoadAssetBundle downloadAB in downLoadAssets)
        {
            string md5 = "";//下载的资源的md5，
            if (m_DownLoadMD5Dic.TryGetValue(downloadAB.FileName, out md5))
            {
                if (MD5Manager.Instance.BuildFileMd5(downloadAB.SaveFilePath) != md5)
                {
                    Debug.Log(string.Format("文件{0}MD5校验失败，即将重新下载", downloadAB.FileName));
                    Patch patch = FindPatchByName(downloadAB.FileName);
                    if (patch != null) downLoadList.Add(patch);
                }
            }
        }
        //全部下载成功
        if(downLoadList.Count <= 0)
        {
            m_DownLoadMD5Dic.Clear();
            if(callBack != null)
            {
                m_StartDownload = false;
                callBack();
            }
            if (LoadOver != null) LoadOver();
        }
        //重新下载
        else
        {
            if (m_TryDownCount >= DOWNLOADCOUNT)
            {
                string allName = "";//记录下载失败的文件名
                m_StartDownload = false;//结束下载
                foreach(Patch patch in downLoadList)
                {
                    allName += patch.Name + ";";

                }
                Debug.LogError("资源重复下载4次，MD5校验都失败，请检查资源：" + allName);
                if (ItemError != null) ItemError(allName);
            }
            else
            {
                m_TryDownCount++;
                m_DownLoadMD5Dic.Clear();//清空，不包含已经下载过的资源
                foreach (Patch patch in downLoadList)
                {
                    m_DownLoadMD5Dic.Add(patch.Name, patch.Md5);//更新为要重新下载的资源Md5
                }
                m_Mono.StartCoroutine(StartDownLoadAB(callBack,downLoadList)); //自动重新下载校验失败的文件
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
