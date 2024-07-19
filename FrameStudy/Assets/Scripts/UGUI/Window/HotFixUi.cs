using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotFixUi : Window
{
    private HotFixPanel m_Panel;

    public override void Awake(params object[] paralist)
    {
        m_Panel = GameObject.GetComponent<HotFixPanel>();
        m_Panel.m_Image.fillAmount = 0;//初始进度为0
        m_Panel.m_Text.text = string.Format("下载中。。。{0:F}M/S", 0);//初始下载进度0M/S；
        HotPatchManager.Instance.ServerInfoError += ServerInfoError;
        HotPatchManager.Instance.ItemError += ItemError;
        HotFix();
    }

    public override void OnClose()
    {
        HotPatchManager.Instance.ServerInfoError -= ServerInfoError;
        HotPatchManager.Instance.ItemError -= ItemError;
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
        }
        else
        {
            //检查当前版本
        }
    }

    void CheckVersion()
    {
        HotPatchManager.Instance.CheckVersion((hot) =>
        {
            if (hot)
            {
                //提示玩家是否确定热更下载
            }
            else
            {
                //直接进入游戏
            }
        });
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
