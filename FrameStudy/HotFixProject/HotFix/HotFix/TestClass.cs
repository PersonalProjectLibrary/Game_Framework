using System;
using UnityEngine;

namespace HotFix
{
    public class TestClass
    {
        /// <summary>
        /// 第一个热更函数
        /// </summary>
        public static void StaticFunTest()
        {
            Debug.Log("TestClass StaticFunTest!!!!!");
        }

        /// <summary>
        /// 简单带参函数
        /// </summary>
        public static void StaticFuncTest2(int a) {
            Debug.Log("TestClass StaticFunTest2 a = " + a);
        }
    }
}
