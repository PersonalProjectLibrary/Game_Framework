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

    public override void Awake(params object[] paralist)
    {
        m_SumTime = 0;
        m_Panel = GameObject.GetComponent<HotFixPanel>();
        m_Panel.m_ProgressImage.fillAmount = 0;//初始进度为0
        m_Panel.m_SpeedText.text = string.Format("{0:F}M/S", 0);//初始下载进度0M/S；
        HotPatchManager.Instance.ServerInfoError += ServerInfoError;
        HotPatchManager.Instance.ItemError += ItemError;
        HotFix();
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
        if(Application.internetReachability == NetworkReachability.NotReachable)//当前网络不正常
        {
            //提示网络错误，检测网络连接是否正常
            GameStart.OpenCommonConfirm("网络连接失败", "网络连接失败，请检查网络连接是否正常", () => { Application.Quit(); }, () => { Application.Quit(); });
        }
        else
        {
            //检查当前版本
            CheckVersion();
        }
    }

    void CheckVersion()
    {
        HotPatchManager.Instance.CheckVersion((hot) =>
        {
            if (hot)
            {
                //提示玩家是否确定热更下载
                GameStart.OpenCommonConfirm("热更确定", string.Format("当前版本为{0}，有{1:F}M大小热更包，是否确定下载？",HotPatchManager.Instance.CurVersion,HotPatchManager.Instance.LoadSumSize/1024.0f), OnClickStartDownLoad, OnClickCancleDownLoad);
            }
            else StartOnFinish();//直接进入游戏
        });
    }

    void OnClickStartDownLoad()
    {
        //手机下，要看是否是数据流量
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)//数据流量网
            {
                GameStart.OpenCommonConfirm("下载确认", "当前使用的是手机流量。是否继续下载", StartDownLoad, OnClickCancleDownLoad);
            }
        }
        else StartDownLoad();//其他平台直接下载
    }

    void OnClickCancleDownLoad()
    {
        Application.Quit();
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
        yield return GameStart.Instance.StartCoroutine(GameStart.Instance.StartGame(m_Panel.m_ProgressImage, m_Panel.m_SliderTopText));
        UIManager.Instance.CloseWnd(this);
    }

    public override void OnUpdate()
    {
        if (HotPatchManager.Instance.StartDownload)
        {
            m_SumTime += Time.deltaTime;
            m_Panel.m_ProgressImage.fillAmount = HotPatchManager.Instance.GetProgress();
            float speed = (HotPatchManager.Instance.GetLoadSize() / 1024.0f) / m_SumTime;
            m_Panel.m_SpeedText.text = string.Format("{0:F} M/S", speed);
        }
    }

    /// <summary>
    /// 服务器错误回调
    /// </summary>
    void ServerInfoError()
    {

    }

    //资源下载失败回调
    void ItemError(string all)
    {

    }
}
