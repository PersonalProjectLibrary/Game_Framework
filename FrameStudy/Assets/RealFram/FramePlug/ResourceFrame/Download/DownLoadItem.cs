using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class DownLoadItem
{
    /// <summary>
    /// 网络资源路径URL
    /// </summary>
    protected string m_Url;
    /// <summary>
    /// 网络资源路径URL
    /// </summary>
    public string Url { get { return m_Url; } }

    /// <summary>
    /// 资源下载存放路径，
    /// 路径中不包含文件名
    /// </summary>
    protected string m_SavePath;
    /// <summary>
    /// 资源下载存放路径，
    /// 路径中不包含文件名
    /// </summary>
    public string SavePath { get { return m_SavePath; } }

    /// <summary>
    /// 文件名
    /// 不包含后缀
    /// </summary>
    protected string m_FileNameWithoutExt;
    /// <summary>
    /// 文件名
    /// 不包含后缀
    /// </summary>
    public string FileNameWithoutExt { get { return m_FileNameWithoutExt; } }

    /// <summary>
    /// 文件后缀
    /// </summary>
    protected string m_FileExt;
    /// <summary>
    /// 文件后缀
    /// </summary>
    public string FileExt { get { return m_FileExt; } }

    /// <summary>
    /// 文件名
    /// 包含后缀
    /// </summary>
    protected string m_FileName;
    /// <summary>
    /// 文件名
    /// 包含后缀
    /// </summary>
    public string FileName { get { return m_FileName; } }

    /// <summary>
    /// 下载文件全路径
    /// 路径+文件名+后缀
    /// </summary>
    protected string m_SaveFilePath;
    /// <summary>
    /// 下载文件全路径
    /// 路径+文件名+后缀
    /// </summary>
    public string SaveFilePath {  get { return m_SaveFilePath; } }

    /// <summary>
    /// 源文件大小
    /// </summary>
    protected long m_FileLength;
    /// <summary>
    /// 源文件大小
    /// </summary>
    public long FileLength {  get { return m_FileLength; } }

    /// <summary>
    /// 当前下载的大小
    /// </summary>
    protected long m_CurLength;
    /// <summary>
    /// 当前下载的大小
    /// </summary>
    public long CurLength {  get { return m_CurLength; } }

    /// <summary>
    /// 是否开始下载
    /// </summary>
    protected bool m_StartDownLoad;
    /// <summary>
    /// 是否开始下载
    /// </summary>
    public bool StartDownLoad { get { return m_StartDownLoad; } }

    /// <summary>
    /// 构造函数、初始化脚本（功能已实现）
    /// </summary>
    /// <param name="url">网络资源路径URL：m_Url</param>
    /// <param name="path">下载存放路径，不包含文件名</param>
    public DownLoadItem(string url, string path)
    {
        m_Url = url;
        m_SavePath = path;
        m_FileNameWithoutExt = Path.GetFileNameWithoutExtension(m_Url);
        m_FileExt = Path.GetExtension(m_Url);
        m_FileName = string.Format("{0}{1}",m_FileNameWithoutExt,m_FileExt);
        m_SaveFilePath = string.Format("{0}/{1}{2}",m_SavePath,m_FileNameWithoutExt,m_FileExt);
        m_StartDownLoad = false;
    }

    /// <summary>
    /// 协程虚函数
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public virtual IEnumerator Download(Action callback = null)
    {
        yield return null;
    }

    /// <summary>
    /// 获取下载进度
    /// </summary>
    /// <returns></returns>
    public abstract float GetProcess();

    /// <summary>
    /// 获取当前下载的文件大小
    /// </summary>
    /// <returns></returns>
    public abstract long GetCurLength();

    /// <summary>
    /// 获取下载的文件大小
    /// </summary>
    /// <returns></returns>
    public abstract long GetLength();

    /// <summary>
    /// 关掉、删除掉当前类
    /// </summary>
    public abstract void Destory();

    /// <summary>
    /// 关闭下载请求
    /// </summary>
    public abstract void DestoryDownload();
}
