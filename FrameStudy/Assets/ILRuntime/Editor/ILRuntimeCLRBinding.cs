#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
[System.Reflection.Obfuscation(Exclude = true)]
public class ILRuntimeCLRBinding
{
    private const string GeneratePath = "Assets/ILRuntime/Generated";
    private const string DLLPATH = "Assets/GameData/Data/HotFix/HotFix.dll.bytes";

    //自己根据热更里面使用的类型进行自主添加的绑定
    [MenuItem("ILRuntime/Generate CLR Binding Code")]
    static void GenerateCLRBinding()
    {
        //生成重定向的函数
        List<Type> types = new List<Type>();
        types.Add(typeof(int));
        types.Add(typeof(float));
        types.Add(typeof(long));
        types.Add(typeof(object));
        types.Add(typeof(string));
        types.Add(typeof(Array));
        types.Add(typeof(Vector2));
        types.Add(typeof(Vector3));
        types.Add(typeof(Quaternion));
        types.Add(typeof(GameObject));
        types.Add(typeof(UnityEngine.Object));
        types.Add(typeof(Transform));
        types.Add(typeof(RectTransform));
        types.Add(typeof(CLRBindingTestClass));
        types.Add(typeof(Time));
        types.Add(typeof(Debug));
        //所有DLL内的类型的真实C#类型都是ILTypeInstance
        types.Add(typeof(List<ILRuntime.Runtime.Intepreter.ILTypeInstance>));

        //生成的类文件放到GeneratePath里
        ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(types, GeneratePath);

        AssetDatabase.Refresh();//刷新下编辑器，使运行生成后能看到运行的效果
    }

    //根据热更dll使用的类型，自动进行全部绑定(建议使用)
    [MenuItem("ILRuntime/Generate CLR Binding Code by Analysis")]
    static void GenerateCLRBindingByAnalysis()
    {
        //用新的分析热更dll调用引用来生成绑定代码，放到DLLPATH目录文件夹里
        ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
        System.IO.FileStream fs = new System.IO.FileStream(DLLPATH, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        try
        {
            domain.LoadAssembly(fs);
        }
        catch (Exception e)
        {
            Debug.LogError("CLR绑定失败\n" + e);
        }
        //Crossbind Adaptor is needed to generate the correct binding code
        InitILRuntime(domain);
        ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain, GeneratePath);

        AssetDatabase.Refresh();
    }

    static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain domain)
    {
        //注册适配器
        ////这里需要注册所有热更DLL中用到的跨域继承Adapter，否则无法正确抓取引用
        domain.RegisterCrossBindingAdaptor(new InheritanceAdapter());
        domain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
        domain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
        domain.RegisterCrossBindingAdaptor(new WindowAdapter());
    }
}
#endif
