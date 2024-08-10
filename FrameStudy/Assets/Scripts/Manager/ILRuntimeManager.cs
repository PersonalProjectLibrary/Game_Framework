using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILRuntime.Runtime.Enviorment;
using System.IO;
using System;

public class ILRuntimeManager : Singleton<ILRuntimeManager>
{
    /// <summary>
    /// 整个工程只有一个ILRuntime的AppDomain
    /// </summary>
    ILRuntime.Runtime.Enviorment.AppDomain m_AppDomain;
    private const string DLLPATH = "Assets/GameData/Data/HotFix/HotFix.dll.bytes";
    private const string PDBPATH = "Assets/GameData/Data/HotFix/HotFix.pdb.bytes";

    /// <summary>
    /// 代码热更初始化
    /// </summary>
    public void Init()
    {
        LoadHotFixAssembly();
    }

    /// <summary>
    /// 加载热更程序集
    /// </summary>
    void LoadHotFixAssembly()
    {
        m_AppDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
        //读取热更资源的dll
        TextAsset dllText = ResourceManager.Instance.LoadResource<TextAsset>(DLLPATH);
        //读取PDB文件，调试数据库，主要用于日志报错
        TextAsset pdbText = ResourceManager.Instance.LoadResource<TextAsset>(PDBPATH);
        //读取加载热更库

        MemoryStream md = new MemoryStream(dllText.bytes);
        MemoryStream mp = new MemoryStream(pdbText.bytes);
        try
        {
            m_AppDomain.LoadAssembly(md, mp, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
        }
        catch
        {
            Debug.LogError("加载热更DLL失败，请确保已经通过VS打开HotFixProject/HotFix.sln编译过热更DLL");
        }
        InitializeILRuntime();
        OnHotFixLoaded();
    }

    #region Old LoadHotFixAssembly
    //void LoadHotFixAssembly()
    //{
    //    m_AppDomain = new AppDomain();
    //    //读取热更资源的dll
    //    TextAsset dllText = ResourceManager.Instance.LoadResource<TextAsset>(DLLPATH);
    //    //读取PDB文件，调试数据库，主要用于日志报错
    //    TextAsset pdbText = ResourceManager.Instance.LoadResource<TextAsset>(PDBPATH);
    //    //读取加载热更库
    //    using (MemoryStream md = new MemoryStream(dllText.txt))//加载dll的流被关闭了。新版要求流不能关闭，也不能用using写法
    //    {
    //        using (MemoryStream mp = new MemoryStream(pdbText.txt))
    //        {
    //            //视频里旧版是m_AppDomain.LoadAssembly(mp, md, new Mono.Cecil.Pdb.PdbReaderProvider());
    //            //当前版本格式（有对比Demo里的使用）
    //            m_AppDomain.LoadAssembly(mp, md, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
    //        }
    //    }
    //    InitializeILRuntime();
    //    OnHotFixLoaded();
    //}
    #endregion

    /// <summary>
    /// 初始化注册ILRuntime的一些注册
    /// </summary>
    void InitializeILRuntime(){}

    /// <summary>
    /// 用于HotFix的热更加载
    /// </summary>
    void OnHotFixLoaded()
    {
        Debug.Log("测试");
        //第一个简单方法的调用
        m_AppDomain.Invoke("HotFix.TestClass", "StaticFunTest", null,null);
    }
}
