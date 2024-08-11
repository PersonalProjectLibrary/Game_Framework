using System;
using UnityEngine;

namespace HotFix
{
    public class TestClass
    {
        #region 可直接调用的静态方法函数
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
        public static void StaticFuncTest2(int a)
        {
            Debug.Log("TestClass StaticFunTest2 a = " + a);
        }
        #endregion

        //用于实例化此类的构造函数
        private int m_Id = 0;//默认为0
        public int ID { get { return m_Id; } }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public TestClass() {
            Debug.Log("无参构造，ID = " + ID);
        }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="id"></param>
        public TestClass(int id){
            m_Id = id;
            Debug.Log("带参构造，ID = " + ID);
        }
    }
}
