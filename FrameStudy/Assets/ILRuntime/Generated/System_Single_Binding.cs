using System;
using System.Collections.Generic;
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
    unsafe class System_Single_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(System.Single);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("IsFinite", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsFinite_0);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("IsInfinity", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsInfinity_1);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("IsNaN", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsNaN_2);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("IsNegative", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsNegative_3);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("IsNegativeInfinity", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsNegativeInfinity_4);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("IsNormal", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsNormal_5);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("IsPositiveInfinity", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsPositiveInfinity_6);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("IsSubnormal", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsSubnormal_7);
            args = new Type[]{typeof(System.Object)};
            method = type.GetMethod("CompareTo", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CompareTo_8);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("CompareTo", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CompareTo_9);
            args = new Type[]{typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("op_Equality", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, op_Equality_10);
            args = new Type[]{typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("op_Inequality", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, op_Inequality_11);
            args = new Type[]{typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("op_LessThan", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, op_LessThan_12);
            args = new Type[]{typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("op_GreaterThan", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, op_GreaterThan_13);
            args = new Type[]{typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("op_LessThanOrEqual", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, op_LessThanOrEqual_14);
            args = new Type[]{typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("op_GreaterThanOrEqual", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, op_GreaterThanOrEqual_15);
            args = new Type[]{typeof(System.Object)};
            method = type.GetMethod("Equals", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Equals_16);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("Equals", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Equals_17);
            args = new Type[]{};
            method = type.GetMethod("GetHashCode", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetHashCode_18);
            args = new Type[]{};
            method = type.GetMethod("ToString", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ToString_19);
            args = new Type[]{typeof(System.IFormatProvider)};
            method = type.GetMethod("ToString", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ToString_20);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("ToString", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ToString_21);
            args = new Type[]{typeof(System.String), typeof(System.IFormatProvider)};
            method = type.GetMethod("ToString", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ToString_22);
            //args = new Type[]{typeof(System.Span<System.Char>), typeof(System.Int32).MakeByRefType(), typeof(System.ReadOnlySpan<System.Char>), typeof(System.IFormatProvider)};
            //method = type.GetMethod("TryFormat", flag, null, args, null);
            //app.RegisterCLRMethodRedirection(method, TryFormat_23);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("Parse", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Parse_24);
            args = new Type[]{typeof(System.String), typeof(System.Globalization.NumberStyles)};
            method = type.GetMethod("Parse", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Parse_25);
            args = new Type[]{typeof(System.String), typeof(System.IFormatProvider)};
            method = type.GetMethod("Parse", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Parse_26);
            args = new Type[]{typeof(System.String), typeof(System.Globalization.NumberStyles), typeof(System.IFormatProvider)};
            method = type.GetMethod("Parse", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Parse_27);
            //args = new Type[]{typeof(System.ReadOnlySpan<System.Char>), typeof(System.Globalization.NumberStyles), typeof(System.IFormatProvider)};
            //method = type.GetMethod("Parse", flag, null, args, null);
            //app.RegisterCLRMethodRedirection(method, Parse_28);
            args = new Type[]{typeof(System.String), typeof(System.Single).MakeByRefType()};
            method = type.GetMethod("TryParse", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, TryParse_29);
            //args = new Type[]{typeof(System.ReadOnlySpan<System.Char>), typeof(System.Single).MakeByRefType()};
            //method = type.GetMethod("TryParse", flag, null, args, null);
            //app.RegisterCLRMethodRedirection(method, TryParse_30);
            args = new Type[]{typeof(System.String), typeof(System.Globalization.NumberStyles), typeof(System.IFormatProvider), typeof(System.Single).MakeByRefType()};
            method = type.GetMethod("TryParse", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, TryParse_31);
            //args = new Type[]{typeof(System.ReadOnlySpan<System.Char>), typeof(System.Globalization.NumberStyles), typeof(System.IFormatProvider), typeof(System.Single).MakeByRefType()};
            //method = type.GetMethod("TryParse", flag, null, args, null);
            //app.RegisterCLRMethodRedirection(method, TryParse_32);
            args = new Type[]{};
            method = type.GetMethod("GetTypeCode", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetTypeCode_33);

            field = type.GetField("MinValue", flag);
            app.RegisterCLRFieldGetter(field, get_MinValue_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_MinValue_0, null);
            field = type.GetField("Epsilon", flag);
            app.RegisterCLRFieldGetter(field, get_Epsilon_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_Epsilon_1, null);
            field = type.GetField("MaxValue", flag);
            app.RegisterCLRFieldGetter(field, get_MaxValue_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_MaxValue_2, null);
            field = type.GetField("PositiveInfinity", flag);
            app.RegisterCLRFieldGetter(field, get_PositiveInfinity_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_PositiveInfinity_3, null);
            field = type.GetField("NegativeInfinity", flag);
            app.RegisterCLRFieldGetter(field, get_NegativeInfinity_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_NegativeInfinity_4, null);
            field = type.GetField("NaN", flag);
            app.RegisterCLRFieldGetter(field, get_NaN_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_NaN_5, null);


            app.RegisterCLRCreateArrayInstance(type, s => new System.Single[s]);


        }

        static System.Single GetInstance(ILRuntime.Runtime.Enviorment.AppDomain __domain, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ptr_of_this_method = ILIntepreter.GetObjectAndResolveReference(ptr_of_this_method);
            System.Single instance_of_this_method;
            switch(ptr_of_this_method->ObjectType)
            {
                case ObjectTypes.FieldReference:
                    {
                        var instance_of_fieldReference = __mStack[ptr_of_this_method->Value];
                        if(instance_of_fieldReference is ILTypeInstance)
                        {
                            instance_of_this_method = (System.Single)typeof(System.Single).CheckCLRTypes(((ILTypeInstance)instance_of_fieldReference)[ptr_of_this_method->ValueLow]);
                        }
                        else
                        {
                            var t = __domain.GetType(instance_of_fieldReference.GetType()) as CLRType;
                            instance_of_this_method = (System.Single)t.GetFieldValue(ptr_of_this_method->ValueLow, instance_of_fieldReference);
                        }
                    }
                    break;
                case ObjectTypes.StaticFieldReference:
                    {
                        var t = __domain.GetType(ptr_of_this_method->Value);
                        if(t is ILType)
                        {
                            instance_of_this_method = (System.Single)typeof(System.Single).CheckCLRTypes(((ILType)t).StaticInstance[ptr_of_this_method->ValueLow]);
                        }
                        else
                        {
                            instance_of_this_method = (System.Single)((CLRType)t).GetFieldValue(ptr_of_this_method->ValueLow, null);
                        }
                    }
                    break;
                case ObjectTypes.ArrayReference:
                    {
                        var instance_of_arrayReference = __mStack[ptr_of_this_method->Value] as System.Single[];
                        instance_of_this_method = instance_of_arrayReference[ptr_of_this_method->ValueLow];                        
                    }
                    break;
                default:
                    instance_of_this_method = *(float*)&ptr_of_this_method->Value;
                    break;
            }
            return instance_of_this_method;
        }

        static StackObject* IsFinite_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @f = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = System.Single.IsFinite(@f);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* IsInfinity_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @f = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = System.Single.IsInfinity(@f);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* IsNaN_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @f = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = System.Single.IsNaN(@f);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* IsNegative_3(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @f = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = System.Single.IsNegative(@f);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* IsNegativeInfinity_4(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @f = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = System.Single.IsNegativeInfinity(@f);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* IsNormal_5(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @f = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = System.Single.IsNormal(@f);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* IsPositiveInfinity_6(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @f = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = System.Single.IsPositiveInfinity(@f);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* IsSubnormal_7(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @f = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = System.Single.IsSubnormal(@f);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* CompareTo_8(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object @value = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single instance_of_this_method = GetInstance(__domain, ptr_of_this_method, __mStack);

            var result_of_this_method = instance_of_this_method.CompareTo(@value);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* CompareTo_9(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @value = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single instance_of_this_method = GetInstance(__domain, ptr_of_this_method, __mStack);

            var result_of_this_method = instance_of_this_method.CompareTo(@value);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* op_Equality_10(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @right = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @left = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = left == right;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* op_Inequality_11(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @right = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @left = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = left != right;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* op_LessThan_12(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @right = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @left = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = left < right;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* op_GreaterThan_13(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @right = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @left = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = left > right;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* op_LessThanOrEqual_14(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @right = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @left = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = left <= right;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* op_GreaterThanOrEqual_15(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @right = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @left = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = left >= right;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* Equals_16(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object @obj = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single instance_of_this_method = GetInstance(__domain, ptr_of_this_method, __mStack);

            var result_of_this_method = instance_of_this_method.Equals(@obj);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* Equals_17(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @obj = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single instance_of_this_method = GetInstance(__domain, ptr_of_this_method, __mStack);

            var result_of_this_method = instance_of_this_method.Equals(@obj);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* GetHashCode_18(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single instance_of_this_method = GetInstance(__domain, ptr_of_this_method, __mStack);

            var result_of_this_method = instance_of_this_method.GetHashCode();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* ToString_19(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single instance_of_this_method = GetInstance(__domain, ptr_of_this_method, __mStack);

            var result_of_this_method = instance_of_this_method.ToString();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ToString_20(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.IFormatProvider @provider = (System.IFormatProvider)typeof(System.IFormatProvider).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single instance_of_this_method = GetInstance(__domain, ptr_of_this_method, __mStack);

            var result_of_this_method = instance_of_this_method.ToString(@provider);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ToString_21(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @format = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single instance_of_this_method = GetInstance(__domain, ptr_of_this_method, __mStack);

            var result_of_this_method = instance_of_this_method.ToString(@format);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ToString_22(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.IFormatProvider @provider = (System.IFormatProvider)typeof(System.IFormatProvider).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @format = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single instance_of_this_method = GetInstance(__domain, ptr_of_this_method, __mStack);

            var result_of_this_method = instance_of_this_method.ToString(@format, @provider);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        /*
        static StackObject* TryFormat_23(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.IFormatProvider @provider = (System.IFormatProvider)typeof(System.IFormatProvider).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.ReadOnlySpan<System.Char> @format = (System.ReadOnlySpan<System.Char>)typeof(System.ReadOnlySpan<System.Char>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)16);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @charsWritten = __intp.RetriveInt32(ptr_of_this_method, __mStack);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Span<System.Char> @destination = (System.Span<System.Char>)typeof(System.Span<System.Char>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)16);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            System.Single instance_of_this_method = GetInstance(__domain, ptr_of_this_method, __mStack);

            var result_of_this_method = instance_of_this_method.TryFormat(@destination, out @charsWritten, @format, @provider);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            __intp.FreeStackValueType(ptr_of_this_method);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            switch(ptr_of_this_method->ObjectType)
            {
                case ObjectTypes.StackObjectReference:
                    {
                        var ___dst = ILIntepreter.ResolveReference(ptr_of_this_method);
                        ___dst->ObjectType = ObjectTypes.Integer;
                        ___dst->Value = @charsWritten;
                    }
                    break;
                case ObjectTypes.FieldReference:
                    {
                        var ___obj = __mStack[ptr_of_this_method->Value];
                        if(___obj is ILTypeInstance)
                        {
                            ((ILTypeInstance)___obj)[ptr_of_this_method->ValueLow] = @charsWritten;
                        }
                        else
                        {
                            var ___type = __domain.GetType(___obj.GetType()) as CLRType;
                            ___type.SetFieldValue(ptr_of_this_method->ValueLow, ref ___obj, @charsWritten);
                        }
                    }
                    break;
                case ObjectTypes.StaticFieldReference:
                    {
                        var ___type = __domain.GetType(ptr_of_this_method->Value);
                        if(___type is ILType)
                        {
                            ((ILType)___type).StaticInstance[ptr_of_this_method->ValueLow] = @charsWritten;
                        }
                        else
                        {
                            ((CLRType)___type).SetStaticFieldValue(ptr_of_this_method->ValueLow, @charsWritten);
                        }
                    }
                    break;
                 case ObjectTypes.ArrayReference:
                    {
                        var instance_of_arrayReference = __mStack[ptr_of_this_method->Value] as System.Int32[];
                        instance_of_arrayReference[ptr_of_this_method->ValueLow] = @charsWritten;
                    }
                    break;
            }

            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            __intp.FreeStackValueType(ptr_of_this_method);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            __intp.Free(ptr_of_this_method);
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }
        //*/

        static StackObject* Parse_24(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @s = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Single.Parse(@s);

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* Parse_25(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Globalization.NumberStyles @style = (System.Globalization.NumberStyles)typeof(System.Globalization.NumberStyles).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @s = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Single.Parse(@s, @style);

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* Parse_26(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.IFormatProvider @provider = (System.IFormatProvider)typeof(System.IFormatProvider).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @s = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Single.Parse(@s, @provider);

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* Parse_27(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.IFormatProvider @provider = (System.IFormatProvider)typeof(System.IFormatProvider).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Globalization.NumberStyles @style = (System.Globalization.NumberStyles)typeof(System.Globalization.NumberStyles).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.String @s = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Single.Parse(@s, @style, @provider);

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        /*
        static StackObject* Parse_28(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.IFormatProvider @provider = (System.IFormatProvider)typeof(System.IFormatProvider).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Globalization.NumberStyles @style = (System.Globalization.NumberStyles)typeof(System.Globalization.NumberStyles).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.ReadOnlySpan<System.Char> @s = (System.ReadOnlySpan<System.Char>)typeof(System.ReadOnlySpan<System.Char>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)16);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Single.Parse(@s, @style, @provider);

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }
        //*/

        static StackObject* TryParse_29(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @result = __intp.RetriveFloat(ptr_of_this_method, __mStack);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @s = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);


            var result_of_this_method = System.Single.TryParse(@s, out @result);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            switch(ptr_of_this_method->ObjectType)
            {
                case ObjectTypes.StackObjectReference:
                    {
                        var ___dst = ILIntepreter.ResolveReference(ptr_of_this_method);
                        ___dst->ObjectType = ObjectTypes.Float;
                        *(float*)&___dst->Value = @result;
                    }
                    break;
                case ObjectTypes.FieldReference:
                    {
                        var ___obj = __mStack[ptr_of_this_method->Value];
                        if(___obj is ILTypeInstance)
                        {
                            ((ILTypeInstance)___obj)[ptr_of_this_method->ValueLow] = @result;
                        }
                        else
                        {
                            var ___type = __domain.GetType(___obj.GetType()) as CLRType;
                            ___type.SetFieldValue(ptr_of_this_method->ValueLow, ref ___obj, @result);
                        }
                    }
                    break;
                case ObjectTypes.StaticFieldReference:
                    {
                        var ___type = __domain.GetType(ptr_of_this_method->Value);
                        if(___type is ILType)
                        {
                            ((ILType)___type).StaticInstance[ptr_of_this_method->ValueLow] = @result;
                        }
                        else
                        {
                            ((CLRType)___type).SetStaticFieldValue(ptr_of_this_method->ValueLow, @result);
                        }
                    }
                    break;
                 case ObjectTypes.ArrayReference:
                    {
                        var instance_of_arrayReference = __mStack[ptr_of_this_method->Value] as System.Single[];
                        instance_of_arrayReference[ptr_of_this_method->ValueLow] = @result;
                    }
                    break;
            }

            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            __intp.Free(ptr_of_this_method);
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        /*
        static StackObject* TryParse_30(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @result = __intp.RetriveFloat(ptr_of_this_method, __mStack);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.ReadOnlySpan<System.Char> @s = (System.ReadOnlySpan<System.Char>)typeof(System.ReadOnlySpan<System.Char>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)16);


            var result_of_this_method = System.Single.TryParse(@s, out @result);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            switch(ptr_of_this_method->ObjectType)
            {
                case ObjectTypes.StackObjectReference:
                    {
                        var ___dst = ILIntepreter.ResolveReference(ptr_of_this_method);
                        ___dst->ObjectType = ObjectTypes.Float;
                        *(float*)&___dst->Value = @result;
                    }
                    break;
                case ObjectTypes.FieldReference:
                    {
                        var ___obj = __mStack[ptr_of_this_method->Value];
                        if(___obj is ILTypeInstance)
                        {
                            ((ILTypeInstance)___obj)[ptr_of_this_method->ValueLow] = @result;
                        }
                        else
                        {
                            var ___type = __domain.GetType(___obj.GetType()) as CLRType;
                            ___type.SetFieldValue(ptr_of_this_method->ValueLow, ref ___obj, @result);
                        }
                    }
                    break;
                case ObjectTypes.StaticFieldReference:
                    {
                        var ___type = __domain.GetType(ptr_of_this_method->Value);
                        if(___type is ILType)
                        {
                            ((ILType)___type).StaticInstance[ptr_of_this_method->ValueLow] = @result;
                        }
                        else
                        {
                            ((CLRType)___type).SetStaticFieldValue(ptr_of_this_method->ValueLow, @result);
                        }
                    }
                    break;
                 case ObjectTypes.ArrayReference:
                    {
                        var instance_of_arrayReference = __mStack[ptr_of_this_method->Value] as System.Single[];
                        instance_of_arrayReference[ptr_of_this_method->ValueLow] = @result;
                    }
                    break;
            }

            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            __intp.FreeStackValueType(ptr_of_this_method);
            __intp.Free(ptr_of_this_method);
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }
        //*/

        static StackObject* TryParse_31(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @result = __intp.RetriveFloat(ptr_of_this_method, __mStack);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.IFormatProvider @provider = (System.IFormatProvider)typeof(System.IFormatProvider).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Globalization.NumberStyles @style = (System.Globalization.NumberStyles)typeof(System.Globalization.NumberStyles).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.String @s = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);


            var result_of_this_method = System.Single.TryParse(@s, @style, @provider, out @result);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            switch(ptr_of_this_method->ObjectType)
            {
                case ObjectTypes.StackObjectReference:
                    {
                        var ___dst = ILIntepreter.ResolveReference(ptr_of_this_method);
                        ___dst->ObjectType = ObjectTypes.Float;
                        *(float*)&___dst->Value = @result;
                    }
                    break;
                case ObjectTypes.FieldReference:
                    {
                        var ___obj = __mStack[ptr_of_this_method->Value];
                        if(___obj is ILTypeInstance)
                        {
                            ((ILTypeInstance)___obj)[ptr_of_this_method->ValueLow] = @result;
                        }
                        else
                        {
                            var ___type = __domain.GetType(___obj.GetType()) as CLRType;
                            ___type.SetFieldValue(ptr_of_this_method->ValueLow, ref ___obj, @result);
                        }
                    }
                    break;
                case ObjectTypes.StaticFieldReference:
                    {
                        var ___type = __domain.GetType(ptr_of_this_method->Value);
                        if(___type is ILType)
                        {
                            ((ILType)___type).StaticInstance[ptr_of_this_method->ValueLow] = @result;
                        }
                        else
                        {
                            ((CLRType)___type).SetStaticFieldValue(ptr_of_this_method->ValueLow, @result);
                        }
                    }
                    break;
                 case ObjectTypes.ArrayReference:
                    {
                        var instance_of_arrayReference = __mStack[ptr_of_this_method->Value] as System.Single[];
                        instance_of_arrayReference[ptr_of_this_method->ValueLow] = @result;
                    }
                    break;
            }

            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            __intp.FreeStackValueType(ptr_of_this_method);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            __intp.Free(ptr_of_this_method);
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        /*
        static StackObject* TryParse_32(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @result = __intp.RetriveFloat(ptr_of_this_method, __mStack);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.IFormatProvider @provider = (System.IFormatProvider)typeof(System.IFormatProvider).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Globalization.NumberStyles @style = (System.Globalization.NumberStyles)typeof(System.Globalization.NumberStyles).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.ReadOnlySpan<System.Char> @s = (System.ReadOnlySpan<System.Char>)typeof(System.ReadOnlySpan<System.Char>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)16);


            var result_of_this_method = System.Single.TryParse(@s, @style, @provider, out @result);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            switch(ptr_of_this_method->ObjectType)
            {
                case ObjectTypes.StackObjectReference:
                    {
                        var ___dst = ILIntepreter.ResolveReference(ptr_of_this_method);
                        ___dst->ObjectType = ObjectTypes.Float;
                        *(float*)&___dst->Value = @result;
                    }
                    break;
                case ObjectTypes.FieldReference:
                    {
                        var ___obj = __mStack[ptr_of_this_method->Value];
                        if(___obj is ILTypeInstance)
                        {
                            ((ILTypeInstance)___obj)[ptr_of_this_method->ValueLow] = @result;
                        }
                        else
                        {
                            var ___type = __domain.GetType(___obj.GetType()) as CLRType;
                            ___type.SetFieldValue(ptr_of_this_method->ValueLow, ref ___obj, @result);
                        }
                    }
                    break;
                case ObjectTypes.StaticFieldReference:
                    {
                        var ___type = __domain.GetType(ptr_of_this_method->Value);
                        if(___type is ILType)
                        {
                            ((ILType)___type).StaticInstance[ptr_of_this_method->ValueLow] = @result;
                        }
                        else
                        {
                            ((CLRType)___type).SetStaticFieldValue(ptr_of_this_method->ValueLow, @result);
                        }
                    }
                    break;
                 case ObjectTypes.ArrayReference:
                    {
                        var instance_of_arrayReference = __mStack[ptr_of_this_method->Value] as System.Single[];
                        instance_of_arrayReference[ptr_of_this_method->ValueLow] = @result;
                    }
                    break;
            }

            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            __intp.FreeStackValueType(ptr_of_this_method);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            __intp.FreeStackValueType(ptr_of_this_method);
            __intp.Free(ptr_of_this_method);
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }
        //*/

        static StackObject* GetTypeCode_33(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single instance_of_this_method = GetInstance(__domain, ptr_of_this_method, __mStack);

            var result_of_this_method = instance_of_this_method.GetTypeCode();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_MinValue_0(ref object o)
        {
            return System.Single.MinValue;
        }

        static StackObject* CopyToStack_MinValue_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = System.Single.MinValue;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static object get_Epsilon_1(ref object o)
        {
            return System.Single.Epsilon;
        }

        static StackObject* CopyToStack_Epsilon_1(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = System.Single.Epsilon;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static object get_MaxValue_2(ref object o)
        {
            return System.Single.MaxValue;
        }

        static StackObject* CopyToStack_MaxValue_2(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = System.Single.MaxValue;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static object get_PositiveInfinity_3(ref object o)
        {
            return System.Single.PositiveInfinity;
        }

        static StackObject* CopyToStack_PositiveInfinity_3(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = System.Single.PositiveInfinity;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static object get_NegativeInfinity_4(ref object o)
        {
            return System.Single.NegativeInfinity;
        }

        static StackObject* CopyToStack_NegativeInfinity_4(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = System.Single.NegativeInfinity;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static object get_NaN_5(ref object o)
        {
            return System.Single.NaN;
        }

        static StackObject* CopyToStack_NaN_5(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = System.Single.NaN;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }




    }
}
