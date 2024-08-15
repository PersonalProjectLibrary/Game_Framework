using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix
{
    public class TestCoroutine
    {
        static System.Collections.IEnumerator Coroutine()
        {
            Debug.Log("开始协程，t = "+Time.time);
            yield return new WaitForSeconds(3);
            Debug.Log("协程完成，t = " + Time.time);
        }

        public static void RunTest()
        {
            GameStart.Instance.StartCoroutine(Coroutine());//开启协程
        }
    }
}
