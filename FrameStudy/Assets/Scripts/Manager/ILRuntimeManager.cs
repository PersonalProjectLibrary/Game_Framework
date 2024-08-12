using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILRuntime.Runtime.Enviorment;
using System.IO;
//using System;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;

public class ILRuntimeManager : Singleton<ILRuntimeManager>
{
    /// <summary>
    /// 整个工程只有一个ILRuntime的AppDomain
    /// </summary>
    //ILRuntime.Runtime.Enviorment.AppDomain m_AppDomain;
    AppDomain m_AppDomain;
    private const string DLLPATH = "Assets/GameData/Data/HotFix/HotFix.dll.bytes";
    private const string PDBPATH = "Assets/GameData/Data/HotFix/HotFix.pdb.bytes";

    //测试跨域委托的委托变量
    public TestDelegateMethod DelegateMethod;
    public TestDelegateFunction DelegateFunction;
    public System.Action<string> DelegateAction;

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
        //m_AppDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
        m_AppDomain = new AppDomain();
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
    void InitializeILRuntime()
    {
        //----------------------1、默认的委托注册，直接注册--------------------------------
        //默认的委托注册，仅仅支持系统自带的Action以及Function，
        //使用RegisterMethodDelegate或RegisterFunctionDelegate
        //对应由系统提供的Action委托类型：DelegateAction委托，里面参数是string类型
        m_AppDomain.DelegateManager.RegisterMethodDelegate<string>();

        /* TestDelegateMetho和TestDelegateFunction是自定义委托，非系统默认委托。
        //1、对应自定义委托：void TestDelegateMethod<int>，委托运行会报错
        m_AppDomain.DelegateManager.RegisterMethodDelegate<int>();
        //2、对应自定义委托：string TestDelegateFunction<int,string>，委托运行会报错
        m_AppDomain.DelegateManager.RegisterFunctionDelegate<int,string>();
        //*/

        //---------------------2、自定义委托或Unity委托注册，使用委托转换器---------------------

        //自定义委托或Unity委托注册，使用委托转换器RegisterDelegateConvertor
        //通过Lamada表达式，把目标委托转为系统默认的Method、Function委托
        //注：可参考官方文档--ILRuntime中使用委托
        //创建委托实例的时候ILRuntime选择了显式注册，同一个参数组合的委托，只需要注册一次即可

        //（1）对应void TestDelegateMethod<int>
        m_AppDomain.DelegateManager.RegisterDelegateConvertor<TestDelegateMethod>((a) =>
        {
            return new TestDelegateMethod((b) =>
            {
                ((System.Action<int>)a)(b);
            });
        });
        //委托转换后，对应的系统委托若之前没注册过，也要注册下
        m_AppDomain.DelegateManager.RegisterMethodDelegate<int>();

        //（2）对应 string TestDelegateFunction<int,string>
        m_AppDomain.DelegateManager.RegisterDelegateConvertor<TestDelegateFunction>((a) =>
        {
            return new TestDelegateFunction((b) =>
            {
                return ((System.Func<int,string>)a)(b);//有返回值，故这里是Func<int,string>
            });
        });
        //委托转换后，对应的系统委托若之前没注册过，也要注册下
        m_AppDomain.DelegateManager.RegisterFunctionDelegate<int, string>();
    }

    /// <summary>
    /// 用于HotFix的热更加载
    /// </summary>
    void OnHotFixLoaded()
    {
        #region 热更工程静态方法调用：2种方法，4种写法
        /*
        //第一种热更工程里方法的调用：每次先反射获取方法所在的类，然后再调用反射类里的方法
        m_AppDomain.Invoke("HotFix.TestClass", "StaticFunTest", null, null);

        //第二种方法：先单独获取类，之后一直使用这个类来调用
        IType type = m_AppDomain.LoadedTypes["HotFix.TestClass"];//ILRuntime里的IType可以代表任何一个类
        //根据方法名称和参数个数获取方法，学习获取函数进行调用
        //GetMethod(方法名,方法参数个数)

        //第二种方法--1：调用无参函数
        IMethod method = type.GetMethod("StaticFunTest", 0);//IMethod可以代表类里的任何一个方法
        m_AppDomain.Invoke(method, null, null);//使用程序集，执行目标函数

        //第二种方法--2：调用有参函数（传一个参数）
        IMethod method1 = type.GetMethod("StaticFuncTest2", 1);
        m_AppDomain.Invoke(method1, null, 5);

        //第二种方法--3：调用有参函数（传多参数List）
        IType intType = m_AppDomain.GetType(typeof(int));//获取热更工程里的int类型
        List<IType> paraList = new List<IType>();//参数List
        paraList.Add(intType);
        IMethod method2 = type.GetMethod("StaticFuncTest2", paraList, null);
        m_AppDomain.Invoke(method2, null, 15);
        //*/
        #endregion

        #region 实例化热更工程里的类，类似Unity里new一个类
        //ILRuntime自带一些API来创建类，可以参考官方文档说明
        /*
        //第一种实例化方式：直接实例化
        object obj = m_AppDomain.Instantiate("HotFix.TestClass", null);//输出：无参构造，ID = 0
        object obj2 = m_AppDomain.Instantiate("HotFix.TestClass", new object[] { 25 });//输出：带参构造，ID = 25

        //第二种实例方式：使用IType实例化类，然后使用API创建类，API获取指定属性
        IType type2 = m_AppDomain.LoadedTypes["HotFix.TestClass"];//先获取到类

        //第二种实例方式，方法1：无参的构造
        object obj3 = ((ILType)type2).Instantiate();//输出：无参构造，ID = 0

        //第二种实例方式，方法2：无参构造，注：这里的get是ILRuntime的api
        int id = (int)m_AppDomain.Invoke("HotFix.TestClass", "get_ID", obj3, null);
        Debug.Log("id =" + id);//输出：id =0
        //int id = (int)m_AppDomain.Invoke("HotFix.TestClass", "Get_ID", obj3, null);//会报错

        //第二种实例方式，方法3：带参的构造
        object[] args = new object[1] { 35 };//参数列表
        object obj4 = ((ILType)type2).Instantiate(args);//输出：带参构造，ID = 35

        //第二种实例方式，方法4：带参构造
        int id2 = (int)m_AppDomain.Invoke("HotFix.TestClass", "get_ID", obj4, null);
        Debug.Log("id2 =" + id2);//输出：id2 =35
        //int id2 = (int)m_AppDomain.Invoke("HotFix.TestClass", "get_ID", obj4, args);//会报错
        //int id2 = (int)m_AppDomain.Invoke("HotFix.TestClass", "get_ID", obj4, 55);//会报错
        //*/
        #endregion

        #region 调用泛型方法
        /*
        //第一种调用泛型方法，专门的泛型API：InvokeGenericMethod();
        IType stringType = m_AppDomain.GetType(typeof(string));//获取string类型
        IType[] genericArguments = new IType[] { stringType };//构建用于泛型的string数组
        //InvokeGenericMethod("类全名：库名.类名", "泛型方法名", 泛型数组, 泛型实例, 泛型参数);
        m_AppDomain.InvokeGenericMethod("HotFix.TestClass", "GenericMethod", genericArguments, null, "Ocean");

        //第二种调用泛型方法，使用IMethod()
        IType type3 = m_AppDomain.LoadedTypes["HotFix.TestClass"];//先获取到类
        IType stringType2 = m_AppDomain.GetType(typeof(string));//传参的类型
        IType[] genericArguments2 = new IType[] { stringType2 };//泛型的类型
        List<IType> paraList = new List<IType>() { stringType2 };//传参的List
        //GetMethod("泛型方法名", 传进去的参数列表, 泛型类型);
        IMethod method = type3.GetMethod("GenericMethod", paraList, genericArguments2);
        m_AppDomain.Invoke(method, null, "Ocean2222222222222");//调用方法
        //*/
        #endregion

        #region 热更内部--3种委托调用
        /*
        m_AppDomain.Invoke("HotFix.TestDelegate", "Initialize", null, null);//委托注册
        m_AppDomain.Invoke("HotFix.TestDelegate", "RunTest", null, null);//委托调用
        //*/
        #endregion

        #region 跨域委托--Unity主工程的委托
        //可热更工程里调用，也可Unity里调用，具体哪里调用无所谓，主要委托定义在主工程
        m_AppDomain.Invoke("HotFix.TestDelegate", "Initialize2", null, null);//委托注册
        m_AppDomain.Invoke("HotFix.TestDelegate", "RunTest2", null, null);//委托调用
        #endregion

    }
}

/// <summary>
/// 测试热更内部的3种委托调用的自定义委托
/// </summary>
/// <param name="a"></param>
public delegate void TestDelegateMethod(int a);//普通传参委托
public delegate string TestDelegateFunction(int b);//带返回值的委托
