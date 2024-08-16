using System;
using UnityEngine;

namespace HotFix
{
    public class TestMono
    {
        //执行这个方法，就可以自动执行MonoTest里的Awake、Start、Update方法
        public static void RunTest(GameObject go)
        {
            go.AddComponent<TestMonoBehaviour>();
        }
    }
    public class TestMonoBehaviour : MonoBehaviour
    {
        public float m_CurTime = 0;

        void Awake()
        {
            Debug.Log("MonoTest Awake!");
        }
        void Start()
        {
            Debug.Log("MonoTest Start!");
        }
        void Update()
        {
            if (m_CurTime < 0.2f)
            {
                Debug.Log("MonoTest Update!");
                m_CurTime += Time.deltaTime;
            }
        }

        public static void RunTest(GameObject go)
        {
            Debug.Log("MonoTest RunTest");
        }
    }
}
