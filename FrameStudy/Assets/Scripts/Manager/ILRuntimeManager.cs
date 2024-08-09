using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILRuntime.Runtime.Enviorment;
using System.IO;

public class ILRuntimeManager : Singleton<ILRuntimeManager>
{
    /// <summary>
    /// 整个工程只有一个ILRuntime的AppDomain
    /// </summary>
    AppDomain m_AppDomain;
    private const string DLLPATH = "Assets/GameData/Data/HotFix/HotFix.dll.txt";
    private const string PDBPATH = "Assets/GameData/Data/HotFix/HotFix.pdb.txt";

    public void Init()
    {
        LoadHotFixAssembly();
    }

    void LoadHotFixAssembly()
    {
        m_AppDomain = new AppDomain();
        //读取热更资源的dll
        TextAsset dllText = ResourceManager.Instance.LoadResource<TextAsset>(DLLPATH);
        //读取PDB文件，调试数据库，主要用于日志报错
        TextAsset pdbText = ResourceManager.Instance.LoadResource<TextAsset>(PDBPATH);
        //读取加载热更库
        using(MemoryStream md = new MemoryStream(dllText.bytes))
        {
            using (MemoryStream mp = new MemoryStream(pdbText.bytes))
            {
                //视频里旧版是m_AppDomain.LoadAssembly(mp, md, new Mono.Cecil.Pdb.PdbReaderProvider());
                m_AppDomain.LoadAssembly(mp, md, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
            }
        }
        InitializeILRuntime();
        OnHotFixLoaded();
    }

    /// <summary>
    /// 初始化注册ILRuntime的一些注册
    /// </summary>
    void InitializeILRuntime()
    {

    }

    /// <summary>
    /// 用于HotFix的热更加载
    /// </summary>
    void OnHotFixLoaded()
    {

    }
}
