using System;
using UnityEngine;

namespace HotFix
{
    public class TestDelegate
    {
        //1、使用系统自带的委托
        static Action<string> delegateAction;
        //2、使用Unity项目里的委托
        static TestDelegateMethod delegateMethod;
        static TestDelegateFunction delegateFunction;

        //3、委托里添加的方法函数
        static void Action(string str)
        {
            Debug.Log("TestDelegate Action str =" + str);
        }
        static void Method(int a)
        {
            Debug.Log("TestDelegate Method a =" + a);
        }
        static string Function(int b)
        {
            Debug.Log("TestDelegate Function b =" + b);
            return b.ToString();
        }

        /// <summary>
        /// 4、热更内部--委托注册
        /// </summary>
        public static void Initialize()
        {
            delegateAction = Action;
            delegateMethod = Method;
            delegateFunction = Function;
        }
        /// <summary>
        /// 4、跨域委托--Unity主程序里的委托注册
        /// </summary>
        public static void Initialize2()
        {
            ILRuntimeManager.Instance.DelegateAction = Action;
            ILRuntimeManager.Instance.DelegateMethod = Method;
            ILRuntimeManager.Instance.DelegateFunction = Function;
        }

        /// <summary>
        /// 5、热更内部--委托调用
        /// </summary>
        public static void RunTest()
        {
            if (delegateAction != null) delegateAction("Ocean");
            if (delegateMethod != null) delegateMethod(55);
            if (delegateFunction != null)
            {
                string str = delegateFunction(65);
                Debug.Log("RunTest delegateFunction：" + str);
            }
        }

        /// <summary>
        /// 5、跨域委托--Unity主程序里的委托调用
        /// </summary>
        public static void RunTest2()
        {
            if (ILRuntimeManager.Instance.DelegateAction != null) 
                ILRuntimeManager.Instance.DelegateAction("Ocean");
            if (ILRuntimeManager.Instance.DelegateMethod != null)
                ILRuntimeManager.Instance.DelegateMethod(75);
            if (ILRuntimeManager.Instance.DelegateFunction != null)
            {
                string str = ILRuntimeManager.Instance.DelegateFunction(85);
                Debug.Log("RunTest2 delegateFunction：" + str);
            }
        }
    }
}
