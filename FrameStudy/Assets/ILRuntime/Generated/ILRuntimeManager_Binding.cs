using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;
#if DEBUG && !DISABLE_ILRUNTIME_DEBUG
using AutoList = System.Collections.Generic.List<object>;
#else
using AutoList = ILRuntime.Other.UncheckedList<object>;
#endif
namespace ILRuntime.Runtime.Generated
{
    unsafe class ILRuntimeManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::ILRuntimeManager);

            field = type.GetField("DelegateAction", flag);
            app.RegisterCLRFieldGetter(field, get_DelegateAction_0);
            app.RegisterCLRFieldSetter(field, set_DelegateAction_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_DelegateAction_0, AssignFromStack_DelegateAction_0);
            field = type.GetField("DelegateMethod", flag);
            app.RegisterCLRFieldGetter(field, get_DelegateMethod_1);
            app.RegisterCLRFieldSetter(field, set_DelegateMethod_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_DelegateMethod_1, AssignFromStack_DelegateMethod_1);
            field = type.GetField("DelegateFunction", flag);
            app.RegisterCLRFieldGetter(field, get_DelegateFunction_2);
            app.RegisterCLRFieldSetter(field, set_DelegateFunction_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_DelegateFunction_2, AssignFromStack_DelegateFunction_2);


        }



        static object get_DelegateAction_0(ref object o)
        {
            return ((global::ILRuntimeManager)o).DelegateAction;
        }

        static StackObject* CopyToStack_DelegateAction_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((global::ILRuntimeManager)o).DelegateAction;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_DelegateAction_0(ref object o, object v)
        {
            ((global::ILRuntimeManager)o).DelegateAction = (System.Action<System.String>)v;
        }

        static StackObject* AssignFromStack_DelegateAction_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.String> @DelegateAction = (System.Action<System.String>)typeof(System.Action<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            ((global::ILRuntimeManager)o).DelegateAction = @DelegateAction;
            return ptr_of_this_method;
        }

        static object get_DelegateMethod_1(ref object o)
        {
            return ((global::ILRuntimeManager)o).DelegateMethod;
        }

        static StackObject* CopyToStack_DelegateMethod_1(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((global::ILRuntimeManager)o).DelegateMethod;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_DelegateMethod_1(ref object o, object v)
        {
            ((global::ILRuntimeManager)o).DelegateMethod = (global::TestDelegateMethod)v;
        }

        static StackObject* AssignFromStack_DelegateMethod_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::TestDelegateMethod @DelegateMethod = (global::TestDelegateMethod)typeof(global::TestDelegateMethod).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            ((global::ILRuntimeManager)o).DelegateMethod = @DelegateMethod;
            return ptr_of_this_method;
        }

        static object get_DelegateFunction_2(ref object o)
        {
            return ((global::ILRuntimeManager)o).DelegateFunction;
        }

        static StackObject* CopyToStack_DelegateFunction_2(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((global::ILRuntimeManager)o).DelegateFunction;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_DelegateFunction_2(ref object o, object v)
        {
            ((global::ILRuntimeManager)o).DelegateFunction = (global::TestDelegateFunction)v;
        }

        static StackObject* AssignFromStack_DelegateFunction_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::TestDelegateFunction @DelegateFunction = (global::TestDelegateFunction)typeof(global::TestDelegateFunction).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            ((global::ILRuntimeManager)o).DelegateFunction = @DelegateFunction;
            return ptr_of_this_method;
        }



    }
}
