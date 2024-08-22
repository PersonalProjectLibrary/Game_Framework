using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotFixUi : Window
{
    private HotFixPanel m_Panel;
    /// <summary>
    /// 累计下载时间
    /// </summary>
    private float m_SumTime = 0;

    public override void Awake(object param1 = null, object param2 = null, object param3 = null)
    {
        m_SumTime = 0;
        m_Panel = GameObject.GetComponent<HotFixPanel>();
        m_Panel.m_ProgressImage.fillAmount = 0;//初始进度为0
        m_Panel.m_SpeedText.text = string.Format("{0:F}M/S", 0);//初始下载进度0M/S；
        HotPatchManager.Instance.ServerInfoError += ServerInfoError;
        HotPatchManager.Instance.ItemError += ItemError;

#if UNITY_EDITOR //编辑器下看情况解压
        if(!ResourceManager.Instance.m_LoadFormAssetBundle) HotFix();//不是AB包加载不解压
        else if (HotPatchManager.Instance.ComputeUnpackFile())//AB包加载且解压文件数不为0
        {
            m_Panel.m_SliderTopText.text = "解压中...";
            HotPatchManager.Instance.StartUnPackFile(() => {
                m_SumTime = 0;//重置时间，避免解压后，后面下载速度的内容显示出问题。
                HotFix();
            });
        }
        else HotFix();//解压文件数为0

#elif UNITY_ANDIORD //安卓平台必须解压
        if (HotPatchManager.Instance.ComputeUnpackFile())//AB包加载且解压文件数不为0
        {
            m_Panel.m_SliderTopText.text = "解压中...";
            HotPatchManager.Instance.StartUnPackFile(() => {
                m_SumTime = 0;//重置时间，避免解压后，后面下载速度的内容显示出问题。
                HotFix();
            });
        }
        else HotFix();//解压文件数为0

#else //其他平台不需要解压
        HotFix();
#endif
    }

    public override void OnClose()
    {
        HotPatchManager.Instance.ServerInfoError -= ServerInfoError;
        HotPatchManager.Instance.ItemError -= ItemError;
        GameMapManager.Instance.LoadScene(ConStr.MENUSCENE);//加载场景
    }

    /// <summary>
    /// 检查热更
    /// </summary>
    void HotFix()
    {
        //检查网络环境
        if (Application.internetReachability == NetworkReachability.NotReachable)//当前网络不正常
        {
            //提示网络错误，检测网络连接是否正常
            GameStart.OpenCommonConfirm("网络连接失败",
                "请检查网络连接是否正常。\n点击“确认”按钮，直接退出游戏；\n点击“取消”按钮，非联网下进入游戏", OnClickCancleDownLoad, StartOnFinish);
        }
        else CheckVersion();//检查当前版本
    }

    void CheckVersion()
    {
        HotPatchManager.Instance.CheckVersion((hot) =>
        {
            if (hot)
            {
                //提示玩家是否确定热更下载
                GameStart.OpenCommonConfirm("热更确定", string.Format("当前版本为{0}，\n有{1:F}M大小热更包，是否确定下载？",
                    HotPatchManager.Instance.CurVersion,HotPatchManager.Instance.LoadSumSize/1024.0f), OnClickStartDownLoad,OnClickCancleDownLoad);
            }
            else StartOnFinish();//直接进入游戏
        });
    }

    /// <summary>
    /// 确认是否下载热更资源
    /// </summary>
    void OnClickStartDownLoad()
    {
        //手机下，要看是否是数据流量
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)//数据流量网
            {
                GameStart.OpenCommonConfirm("下载确认", "当前使用的是手机流量。是否继续下载？", StartDownLoad, OnClickCancleDownLoad);
            }
        }
        else StartDownLoad();//其他平台直接下载
    }

    /// <summary>
    /// 点击取消按钮，退出游戏
    /// </summary>
    void OnClickCancleDownLoad()
    {
        Debug.Log("退出游戏");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// 正式开始下载
    /// </summary>
    void StartDownLoad()
    {
        m_Panel.m_SliderTopText.text = "下载中。。。";
        m_Panel.m_InfoPanel.SetActive(true);//显示热更信息面板
        m_Panel.m_HotContentText.text = HotPatchManager.Instance.CurrentPatch.Des;
        GameStart.Instance.StartCoroutine(HotPatchManager.Instance.StartDownLoadAB(StartOnFinish));
    }

    /// <summary>
    /// 下载完成后的执行的回调
    /// 或无下载直接进入游戏的回调
    /// </summary>
    /// 即下载完成后要做什么事
    void StartOnFinish()
    {
        GameStart.Instance.StartCoroutine(OnFinish());
    }

    IEnumerator OnFinish()
    {
        Debug.Log("进入游戏");
        yield return GameStart.Instance.StartCoroutine(GameStart.Instance.StartGame(m_Panel.m_ProgressImage, m_Panel.m_SliderTopText));
        UIManager.Instance.CloseWnd(this);
    }

    public override void OnUpdate()
    {
        if (HotPatchManager.Instance.StartUnPack)
        {
            m_SumTime += Time.deltaTime;
            m_Panel.m_ProgressImage.fillAmount = HotPatchManager.Instance.GetUnPackProgress();
            float speed = (HotPatchManager.Instance.AlreadyUnPackSize / 1024.0f) / m_SumTime;
            m_Panel.m_SpeedText.text = string.Format("{0:F} M/S",speed);
        }

        if (HotPatchManager.Instance.StartDownload)
        {
            m_SumTime += Time.deltaTime;
            m_Panel.m_ProgressImage.fillAmount = HotPatchManager.Instance.GetProgress();
            float speed = (HotPatchManager.Instance.GetLoadSize() / 1024.0f) / m_SumTime;
            m_Panel.m_SpeedText.text = string.Format("{0:F} M/S", speed);
        }
    }

    /// <summary>
    /// 服务器列表下载出错的回调
    /// </summary>
    void ServerInfoError()
    {
        GameStart.OpenCommonConfirm("服务器列表获取失败", "服务器列表获取失败：\n请检查网络连接是否正常？", 
            CheckVersion,() =>
            {
                GameStart.OpenCommonConfirm("服务器列表获取失败",
                    "是否直接进入游戏？\n点击“确认”按钮，非联网下进入游戏；\n点击“取消”按钮，直接退出游戏", 
                    StartOnFinish, OnClickCancleDownLoad);
            });
    }

    /// <summary>
    /// 资源下载到一半出错的回调
    /// </summary>
    /// <param name="all"></param>
    /// 
    void ItemError(string all)
    {
        GameStart.OpenCommonConfirm("资源下载失败", string.Format("{0}\n等资源下载失败，请重新尝试下载！", all), ReDownLoad, OnClickCancleDownLoad);
    }

    /// <summary>
    /// 重新下载
    /// </summary>
    void ReDownLoad()
    {
        Debug.Log("重新下载");
        HotPatchManager.Instance.CheckVersion((hot) =>
        {
            if (hot) StartDownLoad();
            else StartOnFinish();
        });
    }
}
