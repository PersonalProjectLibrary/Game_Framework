using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownLoadAssetBundle : DownLoadItem
{
    UnityWebRequest m_WebRequest;

    public DownLoadAssetBundle(string url,string path) : base(url, path)
    {

    }

    public override IEnumerator Download(Action callback = null)
    {
        m_WebRequest = UnityWebRequest.Get(m_Url);
        m_StartDownLoad =true;
        m_WebRequest.timeout = 30;//30秒后超时
        yield return m_WebRequest.SendWebRequest();
        m_StartDownLoad=false;

        if (m_WebRequest.result == UnityWebRequest.Result.ConnectionError)
            Debug.LogError("Download Error" + m_WebRequest.error);
        else
        {
            byte[] bytes = m_WebRequest.downloadHandler.data;
            FileTool.CreateFile(m_SaveFilePath, bytes);
            if (callback != null) callback();
        }
    }

    /// <summary>
    /// 关闭下载请求
    /// </summary>
    public override void Destory()
    {
        if(m_WebRequest != null)
        {
            m_WebRequest.Dispose();
            m_WebRequest = null;
        }
    }

    /// <summary>
    /// 获取当前下载的大小
    /// </summary>
    /// <returns></returns>
    public override long GetCurLength()
    {
        if (m_WebRequest != null) return (long)m_WebRequest.downloadedBytes;
        return 0;
    }

    /// <summary>
    /// 获取下载的文件大小
    /// 这里不需要，默认返回0
    /// </summary>
    /// <returns></returns>
    public override long GetLength()
    {
        return 0;
    }

    /// <summary>
    /// 获取下载进度
    /// </summary>
    /// <returns></returns>
    public override float GetProcess()
    {
        if (m_WebRequest != null) return (long)m_WebRequest.downloadProgress;
        return 0;
    }
}
