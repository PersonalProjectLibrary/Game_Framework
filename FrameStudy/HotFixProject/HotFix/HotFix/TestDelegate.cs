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
        
        //4、委托注册
        public static void Initialize()
        {
            delegateAction = Action;
            delegateMethod = Method;
            delegateFunction = Function;
        }

        //5、委托调用
        public static void RunTest()
        {
            if (delegateAction != null) delegateAction("Ocean");
            if (delegateMethod != null) delegateMethod(55);
            if (delegateFunction != null)
            {
                string str = delegateFunction(65);
                Debug.Log("Runtest delegateFunction：" + str);
            }
        }
    }
}
