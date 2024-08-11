using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILRuntime.Runtime.Enviorment;
using System.IO;
using System;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;

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
        //第一种热更工程里方法的调用：每次先反射获取方法所在的类，然后再调用反射类里的方法
        m_AppDomain.Invoke("HotFix.TestClass", "StaticFunTest", null,null);

        //第二种方法：先单独获取类，之后一直使用这个类来调用
        IType type = m_AppDomain.LoadedTypes["HotFix.TestClass"];//ILRuntime里的IType可以代表任何一个类
        //根据方法名称和参数个数获取方法，学习获取函数进行调用
        //GetMethod(方法名,方法参数个数)

        //第二种方法--1：调用无参函数
        IMethod method = type.GetMethod("StaticFunTest", 0);//IMethod可以代表类里的任何一个方法
        m_AppDomain.Invoke(method,null,null);//使用程序集，执行目标函数

        //第二种方法--2：调用有参函数（传一个参数）
        IMethod method1 = type.GetMethod("StaticFuncTest2", 1);
        m_AppDomain.Invoke(method1, null, 5);

        //第二种方法--3：调用有参函数（传多参数List）
        IType intType = m_AppDomain.GetType(typeof(int));//获取热更工程里的int类型
        List<IType> paraList = new List<IType>();//参数List
        paraList.Add(intType);
        IMethod method2 = type.GetMethod("StaticFuncTest2", paraList,null);
        m_AppDomain.Invoke(method2 ,null,15);
    }
}
