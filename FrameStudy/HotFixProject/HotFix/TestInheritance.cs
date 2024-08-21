using System;
using UnityEngine;

namespace HotFix
{
    public class TestInheritance : TestInheritanceBase
    {
        public override void TestAbstract(int a)
        {
            Debug.Log("TestInheritance TestAbstract a = " + a);
        }

        public override void TestVirtual(string str)
        {
            base.TestVirtual(str);
            Debug.Log("TestInheritance TestVirtual str = " + str);
        }

        //通过继承类里的静态方法，来实现对继承类的实例化
        public static TestInheritance NewObject()
        {
            return new TestInheritance();
        }
    }
}
