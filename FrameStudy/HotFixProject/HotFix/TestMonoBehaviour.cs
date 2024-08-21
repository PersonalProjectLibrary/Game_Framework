using System;
using UnityEngine;

namespace HotFix
{
    public class MonoTest
    {
        /// <summary>
        /// 测试Addcompont,Awake等MonoBehaviour的方法
        /// </summary>
        /// <param name="go"></param>
        /// 执行这个方法，就可以自动执行MonoTest里的Awake、Start、Update方法
        public static void RunTest(GameObject go)
        {
            go.AddComponent<TestMonoBehaviour>();
        }

        /// <summary>
        /// 测试GetComponent
        /// </summary>
        public static void RunTest2(GameObject go)
        {
            go.AddComponent<TestMonoBehaviour>();//测试用这里先获取组件
            TestMonoBehaviour testMono = go.GetComponent<TestMonoBehaviour>();
            testMono.Test();
        }
    }
    public class TestMonoBehaviour : MonoBehaviour
    {
        public float m_CurTime = 0;

        void Awake()
        {
            Debug.Log("Mono Awake!");
        }
        void Start()
        {
            Debug.Log("Mono Start!");
        }
        void Update()
        {
            if (m_CurTime < 0.2f)
            {
                Debug.Log("Mono Update!");
                m_CurTime += Time.deltaTime;
            }
        }

        public void Test()
        {
            Debug.Log("Mono Test!!!!!!!!!!!!!!!!!");
        }
    }
}
