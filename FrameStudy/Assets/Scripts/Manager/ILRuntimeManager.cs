using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILRuntime.Runtime.Enviorment;
using System.IO;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;

public class ILRuntimeManager : Singleton<ILRuntimeManager>
{
    /// <summary>
    /// 整个工程只有一个ILRuntime的AppDomain
    /// </summary>
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
        #region 加载热更程序集
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
        #endregion

        InitializeILRuntime();
        OnHotFixLoaded();
    }

    /// <summary>
    /// 初始化注册ILRuntime的一些注册
    /// </summary>
    void InitializeILRuntime()
    {
        #region 1、默认的委托注册，直接注册
        //默认的委托注册，仅仅支持系统自带的Action以及Function，
        //使用RegisterMethodDelegate或RegisterFunctionDelegate
        //对应由系统提供的Action委托类型：DelegateAction委托，里面参数是string类型
        m_AppDomain.DelegateManager.RegisterMethodDelegate<string>();

        //错误写法
        /* TestDelegateMetho和TestDelegateFunction是自定义委托，非系统默认委托。
        //1、对应自定义委托：void TestDelegateMethod<int>，委托运行会报错
        m_AppDomain.DelegateManager.RegisterMethodDelegate<int>();
        //2、对应自定义委托：string TestDelegateFunction<int,string>，委托运行会报错
        m_AppDomain.DelegateManager.RegisterFunctionDelegate<int,string>();
        //*/
        #endregion

        #region 2、自定义委托或Unity委托注册，使用委托转换器
        //自定义委托或Unity委托注册，使用委托转换器RegisterDelegateConvertor
        //通过Lamada表达式，把目标委托转为系统默认的Method、Function委托
        //注：可参考官方文档--ILRuntime中使用委托
        //创建委托实例的时候ILRuntime选择了显式注册，同一个参数组合的委托，只需要注册一次即可
        /*
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
                return ((System.Func<int, string>)a)(b);//有返回值，故这里是Func<int,string>
            });
        });
        //委托转换后，对应的系统委托若之前没注册过，也要注册下
        m_AppDomain.DelegateManager.RegisterFunctionDelegate<int, string>();
        //*/
        #endregion

        #region 3、Unity自带事件注册，使用委托转换器
        //注册Unity事件--UnityAction<bool>事件的注册，如Toggle事件
        /*
        m_AppDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<bool>>((a) =>
        {
            return new UnityEngine.Events.UnityAction<bool>((b) =>
            {
                ((System.Action<bool>)a)(b);
            });
        });
        m_AppDomain.DelegateManager.RegisterMethodDelegate<bool>();
        //*/
        #endregion

        #region 跨域继承的注册
        /*
        //InheritanceAdapter继承了CrossBindingAdaptor，
        //所以这里直接.RegisterCrossBindingAdaptor 注册适配器InheritanceAdapter
        m_AppDomain.RegisterCrossBindingAdaptor(new InheritanceAdapter());
        //*/
        #endregion

        #region 注册协程的适配器
        //m_AppDomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
        #endregion

        #region MonoBehaviour适配器
        m_AppDomain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());//MonoBehaviour测试适配器注册
        AddComponentCLRRedirection();//注册MonoBehaviour测试的AddComponent的重定向
        GetCompomentCLRRedirection();//注册MonoBehaviour测试的GetComponent的重定向
        #endregion

        #region CLR绑定注册（放最后执行）
        //只需要注册一次，官方文档上有示例说明
        ILRuntime.Runtime.Generated.CLRBindings.Initialize(m_AppDomain);
        #endregion

    }

    /// <summary>
    /// 用于HotFix的热更加载
    /// </summary>
    void OnHotFixLoaded()
    {
        #region 1、热更工程静态方法调用：2种方法，4种写法
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

        #region 2、实例化热更工程里的类，类似Unity里new一个类
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

        #region 3、调用泛型方法
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

        #region 4、热更内部委托和跨域委托
        //----------------------热更内部--3种委托调用-----------------------------------
        /*
        m_AppDomain.Invoke("HotFix.TestDelegate", "Initialize", null, null);//委托注册
        m_AppDomain.Invoke("HotFix.TestDelegate", "RunTest", null, null);//委托调用
        //*/

        //----------------------跨域委托--Unity主工程的委托----------------------------------
        //可热更工程里调用，也可Unity里调用，具体哪里调用无所谓，主要委托定义在主工程
        /*
        m_AppDomain.Invoke("HotFix.TestDelegate", "Initialize2", null, null);//委托注册
        m_AppDomain.Invoke("HotFix.TestDelegate", "RunTest2", null, null);//委托调用
        //*/

        //--------------------跨域委托注册--Unity里直接使用注册过的热更事件--------------------------
        /*
        m_AppDomain.Invoke("HotFix.TestDelegate", "Initialize2", null, null);//委托注册
        if (DelegateMethod != null) DelegateMethod(666);
        if (DelegateFunction != null)
        {
            string str = DelegateFunction(789);
            Debug.Log("DelegateFuntion：" + str);
        }
        if (DelegateAction != null) DelegateAction("Ocean666");
        //*/
        #endregion

        #region 5、跨域继承调用
        //跨域继承的第一种调用
        /*
        //实例化跨域继承对象
        TestInheritanceBase obj = m_AppDomain.Instantiate<TestInheritanceBase>("HotFix.TestInheritance");
        //调用跨域继承里的方法
        obj.TestAbstract(556);
        obj.TestVirtual("Ocean");
        //*/

        //跨域继承的第二种调用
        /*
        //通过继承类里的静态方法，来实现对继承类的实例化，除了下面方法，也可以使用其他实例化静态方法的方式调用
        TestInheritanceBase obj2 = m_AppDomain.Invoke("HotFix.TestInheritance", "NewObject", null, null) as TestInheritanceBase;
        obj2.TestAbstract(721);
        obj2.TestVirtual("Ocean123");
        //*/
        #endregion

        #region 6、CLR绑定测试
        /*
        long startTime = System.DateTime.Now.Ticks;//当前时间
        m_AppDomain.Invoke("HotFix.TestCLRBinding", "RunTest", null,null);
        Debug.Log("测试绑定CLR前后的运行时间：" + (System.DateTime.Now.Ticks - startTime));
        //绑定前时间：6442163；绑定后时间：3191707。
        //*/
        #endregion

        #region 7、协程适配器测试
        //m_AppDomain.Invoke("HotFix.TestCoroutine", "RunTest", null,null);
        #endregion

        #region 8、MonoBehaviour测试
        //测试AddComponent
        //m_AppDomain.Invoke("HotFix.MonoTest", "RunTest", null, GameStart.Instance.gameObject);
        m_AppDomain.Invoke("HotFix.MonoTest", "RunTest2", null, GameStart.Instance.gameObject);
        #endregion
    }

    #region MonoBehaviour测试需要的重定向操作
    /* HotFix工程测试代码中使用到AddComponent<MonoTest>，而MonoTest这个类不在Unity主工程中
     * 直接AddComponet<MonoTest> 是加不到Object身上去的，所以要做重定向
     * 写方法SetupCLRRedirection()，将HotFix工程里的AddComponent进行一个挟持，转换成Unity工程那边的调用
     * 写完方法后，在InitializeILRuntime()方法里进行调用执行。这个重定向也要放在CLR绑定注册之前执行
    //*/
    /// <summary>
    /// 获取重定向后的AddComponent
    /// </summary>
    unsafe void AddComponentCLRRedirection()
    {
        var arr = typeof(GameObject).GetMethods();//获取GameObject的类型、函数方法
        foreach (var method in arr)//遍历GameObject里的函数方法
        {
            if(method.Name =="AddComponent"&& method.GetGenericArguments().Length == 1)//找只有一个参数的AddComponet方法
            {
                //使用热更程序集对AddComponent进行重定向成目标函数
                m_AppDomain.RegisterCLRMethodRedirection(method,CLR_AddCompontent);
            }
        }
    }

    /// <summary>
    /// 获取重定向后的GetComponent
    /// </summary>
    unsafe void GetCompomentCLRRedirection()
    {
        var arr = typeof(GameObject).GetMethods();//获取GameObject的所有类型
        foreach(var method in arr)//遍历找只有一个参数的GetComponet方法
        {
            if(method.Name =="GetComponent"&&method.GetGenericArguments().Length == 1)
            {
                m_AppDomain.RegisterCLRMethodRedirection(method, CLR_GetComponent);
            }
        }
    }

    /// <summary>
    /// AddComponent重定向
    /// </summary>
    /// <param name="__intp"></param>
    /// <param name="__esp"></param>
    /// <param name="__mStack"></param>
    /// <param name="__method"></param>
    /// <param name="isNewObj"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    /// 用到指针写的不安全委托,参数可以直接拷贝官方文档的重定向示例里给的写法
    private unsafe StackObject* CLR_AddCompontent(ILIntepreter __intp, StackObject* __esp, List<object> __mStack, CLRMethod __method, bool isNewObj)
    {
        AppDomain __domain = __intp.AppDomain;//获取到程序集

        var ptr = __esp - 1;//获取第一个参数
        GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;//获取第一个参数的值
        if (instance == null) throw new System.Exception();
        __intp.Free(ptr);//获取完参数，释放指针

        var genericArgument = __method.GenericArguments;//获取所有泛型变量
        if (genericArgument != null && genericArgument.Length == 1)//AddComponent只有一个参数，对应泛型变量长度为1
        {
            var type = genericArgument[0];//找到目标函数，进行获取
            object res;

            if (type is CLRType) //CLRType说明是Unity主工程里的类型，不需要做处理
            {
                res = instance.AddComponent(type.TypeForCLR);
            }
            else //ILType说明是热更工程里的类型，需要做重定向：new一个AddComponent然后替换
            {
                //实例化热更dll里的类（MonoTest），传false表手动创建类，Unity不允许new一个MonoBehaviour类
                var ilInstance = new ILTypeInstance(type as ILType, false);
                //创建适配器实例，把GameObject添加上了适配器;后面根据适配器里的类来掉热更里的目标类
                var clrInstance = instance.AddComponent<MonoBehaviourAdapter.Adapter>();
                //因为Unity里写的适配器类clrInstance里没有对应的热更类，要做实例的替换，手动赋值
                clrInstance.ILInstance = ilInstance;//实例替换
                clrInstance.AppDomain = __domain;//程序集也替换
                //这个实例默认创建的CLRInstance不是通过AddCompontent出来的有效实例，所以进行下面的实例替换
                ilInstance.CLRInstance = clrInstance;//Instance转换替换

                res = clrInstance.ILInstance;//转换好了进行赋值

                clrInstance.Awake();//真正调用MonoBehaviour的Awake函数，补掉Awake();
            }
            return ILIntepreter.PushObject(ptr, __mStack, res);
        }
        return __esp;
    }

    /// <summary>
    /// GetComponent重定向
    /// </summary>
    /// <param name="__intp"></param>
    /// <param name="__esp"></param>
    /// <param name="__mStack"></param>
    /// <param name="__method"></param>
    /// <param name="isNewObj"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    private unsafe StackObject* CLR_GetComponent(ILIntepreter __intp, StackObject* __esp, List<object> __mStack, CLRMethod __method, bool isNewObj)
    {
        AppDomain __domain = __intp.AppDomain;
        var ptr = __esp - 1;
        GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
        if (instance == null) throw new System.Exception();
        __intp.Free(ptr);

        var genericArgument = __method.GenericArguments;
        if(genericArgument != null && genericArgument.Length == 1)
        {
            var type = genericArgument[0];
            object res = null;
            if (type is CLRType) res = instance.GetComponent(type.TypeForCLR);
            else//这里不同与AddComponent是new一个然后替换
            {
                //把GameObject里的MonoBehaviour的所有的适配器全部找到，然后遍历判断
                var clrInstances = instance.GetComponents<MonoBehaviourAdapter.Adapter>();
                foreach (var clrInstance in clrInstances)
                {
                    if(clrInstance.ILInstance!=null)//判断是否是无效的MonoBehaviour
                    {
                        if(clrInstance.ILInstance.Type == type)//判断是否是目标类型
                        {
                            res = clrInstance.ILInstance;
                            break;
                        }
                    }
                }
            }
            return ILIntepreter.PushObject(ptr,__mStack, res);
        }
        return __esp;
    }

    #endregion
}

#region 说明
/*
Invoke、GeMethod里获取属性ID、Value使用get_ID、get_Value的写法：
是因为编译软件，属性编译后会变成一个方法，前面会加一个下划线，
就转变为：get_属性、set_属性，
所以书写时是这种写法
 */
#endregion

#region 跨域委托、继承适配器、CLR功能、协程适配器、MonoBehaviour适配器测试代码
/// <summary>
/// 测试委托调用的自定义委托
/// </summary>
/// <param name="a"></param>
public delegate void TestDelegateMethod(int a);//普通传参委托
public delegate string TestDelegateFunction(int b);//带返回值的委托

/// <summary>
/// 测试继承用的基类
/// </summary>
public abstract class TestInheritanceBase
{
    /// <summary>
    /// 虚方法
    /// </summary>
    /// <param name="str"></param>
    public virtual void TestVirtual(string str)
    {
        Debug.Log("TestClassBase TestVirtual str = " + str);
    }

    /// <summary>
    /// 虚变量
    /// </summary>
    public virtual int Value { get { return 0; } }
    /// <summary>
    /// 抽象方法
    /// </summary>
    /// <param name="a"></param>
    public abstract void TestAbstract(int a);
}

/// <summary>
/// 跨域继承类的适配器类
/// </summary>
public class InheritanceAdapter : CrossBindingAdaptor
{
    public override System.Type BaseCLRType
    {
        get { return typeof(TestInheritanceBase); }//返回想要继承的类
    }

    public override System.Type AdaptorType
    {
        get { return typeof(Adapter); }//实际的适配器类
    }

    public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adapter(appdomain, instance);//返回新的适配器对象
    }

    //因为跨域继承只有一个Adapter，因避免一个类同时实现多个外部接口
    public override System.Type[] BaseCLRTypes => base.BaseCLRTypes;

    //实际的适配器类
    class Adapter : TestInheritanceBase, CrossBindingAdaptorType
    {
        private AppDomain m_AppDomain;
        private ILTypeInstance m_Instance;
        private IMethod m_TestAbstract;//重写的抽象方法
        private IMethod m_TestVirtual;//重写的虚方法
        private IMethod m_GetValue;//重写的属性
        private IMethod m_ToString;//适配器里必须要重写tostring方法！！！
        private object[] param1 = new object[1];//重写方法时有参数时，使用的辅助数组
        /// <summary>
        /// m_TestVirtual虚函数是否在被调用中
        /// </summary>
        private bool m_TestVirtualInvoking = false;
        private bool m_GetValueInvoking = false;


        public Adapter() { }//无参构造函数
        public Adapter(AppDomain appdomain, ILTypeInstance instance)//有参构造函数
        {
            m_AppDomain = appdomain;
            m_Instance = instance;
        }
        public ILTypeInstance ILInstance
        {
            get { return m_Instance; }
        }

        //在适配器中重写所有需要在热更脚本重写的方法，并且将控制权转移到脚本里去
        /// <summary>
        /// 重写HotFix里TestInheritance的抽象方法TestAbstract()方法
        /// </summary>
        /// <param name="a"></param>
        public override void TestAbstract(int a)
        {
            if (m_TestAbstract == null) m_TestAbstract = m_Instance.Type.GetMethod("TestAbstract", 1);
            //控制权转移
            if (m_TestAbstract != null)
            {
                param1[0] = a;
                m_AppDomain.Invoke(m_TestAbstract, m_Instance, param1);
            }
        }
        /// <summary>
        /// 重写HotFix里TestInheritance的虚方法TestVirtual()方法
        /// </summary>
        /// <param name="str"></param>
        public override void TestVirtual(string str)
        {
            if (m_TestVirtual == null) m_TestVirtual = m_Instance.Type.GetMethod("TestVirtual", 1);
            //必须要设定一个标识位来表示当前是否在调用中，避免不同自身调用自身导致死循环
            if (m_TestVirtual != null && !m_TestVirtualInvoking)
            {
                m_TestVirtualInvoking = true;
                param1[0] = str;
                m_AppDomain.Invoke(m_TestVirtual, m_Instance, param1);
                m_TestVirtualInvoking = false;
            }
            else base.TestVirtual(str);
        }
        /// <summary>
        /// 重写HotFix里TestInheritance的虚变量Value
        /// </summary>
        public override int Value
        {
            get
            {
                if (m_GetValue == null) m_GetValue = m_Instance.Type.GetMethod("get_Value", 1);
                if (m_GetValue != null && !m_GetValueInvoking)
                {
                    m_GetValueInvoking = true;
                    int res = (int)m_AppDomain.Invoke(m_GetValue, m_Instance, null);
                    m_GetValueInvoking = false;
                    return res;
                }
                else return base.Value;
            }
        }
        /// <summary>
        /// 重写ToString,可以不要考虑为什么这样写，官方写法，直接复制到适配器里就好
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (m_ToString == null) m_ToString = m_AppDomain.ObjectType.GetMethod("ToString", 0);
            IMethod m = m_Instance.Type.GetVirtualMethod(m_ToString);
            if (m == null || m is ILMethod) return m_Instance.ToString();
            else return m_Instance.Type.FullName;
        }
    }
}

/// <summary>
/// 测试CLR的类
/// </summary>
public class CLRBindingTestClass
{
    public static float DoSomeTest(int a, float b)
    {
        return a + b;
    }
}

/// <summary>
/// 协程适配器
/// </summary>
public class CoroutineAdapter : CrossBindingAdaptor
{
    public override System.Type BaseCLRType { get { return null; } }//返回想继承的类

    public override System.Type AdaptorType{ get { return typeof(Adaptor); } }//返回适配器类型

    //继承多个接口的时候使用该方法，原则上尽量不要继承多个接口，但是协程本身就是继承多个接口的
    public override System.Type[] BaseCLRTypes
    {
        get
        {
            return new System.Type[]//协程继承的接口
            {
                typeof(IEnumerator),typeof(IEnumerator<System.Object>),typeof(System.IDisposable)
            };
        }
    }

    public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);
    }

    //协程的适配器
    public class Adaptor : CrossBindingAdaptorType, IEnumerator, IEnumerator<System.Object>, System.IDisposable
    {
        private ILTypeInstance m_Instance;
        private AppDomain m_AppDomain;

        private IMethod m_ToString;
        private IMethod m_CurMethod;
        private IMethod m_MoveNextMethod;
        private IMethod m_ResetMethod;
        private IMethod m_DisposeMethod;

        public Adaptor() { }

        public Adaptor(AppDomain appdomain, ILTypeInstance instance)
        {
            m_Instance = instance;
            m_AppDomain = appdomain;
        }
        public ILTypeInstance ILInstance { get { return m_Instance; }}

        public override string ToString()
        {
            if (m_ToString == null) m_ToString = m_AppDomain.ObjectType.GetMethod("ToString", 0);
            IMethod m = m_Instance.Type.GetVirtualMethod(m_ToString);
            if (m == null || m is ILMethod) return m_Instance.ToString();
            else return m_Instance.Type.FullName;
        }

        public object Current
        {
            get
            {
                if (m_CurMethod == null)
                {
                    m_CurMethod = m_Instance.Type.GetMethod("get_Current", 0);
                    if (m_CurMethod == null) //存在上面方法取不到Current的情况
                        m_CurMethod = m_Instance.Type.GetMethod("System.Collections.IEnumerator.get_Current", 0);
                }

                object res = null;
                if (m_CurMethod != null) res = m_AppDomain.Invoke(m_CurMethod, m_Instance, null);
                return res;
            }
        }

        public bool MoveNext()
        {
            if (m_MoveNextMethod == null) m_MoveNextMethod = m_Instance.Type.GetMethod("MoveNext", 0);
            if(m_MoveNextMethod != null)return (bool)m_AppDomain.Invoke(m_MoveNextMethod, m_Instance, null);
            else return false;
        }

        public void Reset()
        {
            if (m_ResetMethod == null) m_ResetMethod = m_Instance.Type.GetMethod("Reset", 0);
            if(m_ResetMethod != null)m_AppDomain.Invoke(m_ResetMethod, m_Instance, null);
        }

        public void Dispose()
        {
            if(m_DisposeMethod == null)
            {
                m_DisposeMethod = m_Instance.Type.GetMethod("Dispose", 0);
                if (m_DisposeMethod == null) m_DisposeMethod = m_Instance.Type.GetMethod("System.IDisposable.Dispose", 0);
            }
            if(m_DisposeMethod!=null)m_AppDomain.Invoke(m_DisposeMethod, m_Instance, null);
        }
    }
}

/// <summary>
/// MonoBehaviour的简单的适配器
/// </summary>
public class MonoBehaviourAdapter : CrossBindingAdaptor
{
    public override System.Type BaseCLRType{ get { return typeof(MonoBehaviour); } }//返回要继承的类

    public override System.Type AdaptorType{ get { return typeof(Adapter); } }//返回适配器

    public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adapter(appdomain, instance);//返回适配器实例化
    }

    public class Adapter: MonoBehaviour,CrossBindingAdaptorType
    {
        private AppDomain m_AppDomain;
        private ILTypeInstance m_Instance;

        private IMethod m_ToString;
        private IMethod m_AwakeMethod;
        private IMethod m_StartMethod;
        private IMethod m_UpdateMethod;

        public Adapter() { }
        public Adapter(AppDomain appdomain, ILTypeInstance instance)
        {
            m_AppDomain = appdomain;
            m_Instance = instance;
        }

        /// <summary>
        /// 对应MonoBehaviour的实例，可get，也可set
        /// </summary>
        public ILTypeInstance ILInstance
        {
            get { return m_Instance; }
            set
            {
                m_Instance = value;
                //实例修改后，原方法函数要重置，避免还是使用之前实例的同名方法函数
                m_AwakeMethod = null;
                m_StartMethod = null;
                m_UpdateMethod = null;
            }
        }

        /// <summary>
        /// 提供可更改程序集的属性，可get，可set
        /// </summary>
        public AppDomain AppDomain
        {
            get { return m_AppDomain; }
            set { m_AppDomain =  value; }
        }

        public override string ToString()
        {
            if (m_ToString == null) m_ToString = m_AppDomain.ObjectType.GetMethod("ToString", 0);
            IMethod m = m_Instance.Type.GetVirtualMethod(m_ToString);
            if (m == null || m is ILMethod) return m_Instance.ToString();
            else return m_Instance.Type.FullName;
        }

        public void Awake()
        {
            if (m_Instance != null)//Awake执行的比较早，可能存在实例为空的情况
            {
                if (m_AwakeMethod == null) m_AwakeMethod = m_Instance.Type.GetMethod("Awake", 0);
                if(m_AwakeMethod != null)m_AppDomain.Invoke(m_AwakeMethod,m_Instance,null);
            }
        }
        public void Start()
        {
            //前面有Awake先执行判断实例是否存在，这里不用再判断m_Instance是否为空了
            if (m_StartMethod == null) m_StartMethod = m_Instance.Type.GetMethod("Start", 0);
            if (m_StartMethod != null) m_AppDomain.Invoke(m_StartMethod, m_Instance, null);
        }
        public void Update()
        {
            if (m_UpdateMethod == null) m_UpdateMethod = m_Instance.Type.GetMethod("Update", 0);
            if (m_UpdateMethod != null) m_AppDomain.Invoke(m_UpdateMethod, m_Instance, null);
        }
    }
}
#endregion