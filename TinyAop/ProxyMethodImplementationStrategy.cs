using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace TinyAop
{
    public class ProxyMethodImplementationStrategy : IProxyImplementationStrategy
    {
        public void Build(TypeBuilder typeBuilder, Type contractType)
        {
            foreach (var method in GetMethodsToProxy(contractType))
            {
                BuildMethodProxy(typeBuilder, method);
            }
        }

        private IEnumerable<MethodInfo> GetMethodsToProxy(Type contractType)
        {
            var declaredMethods = contractType.GetMethods();
            var inheritedMethods = contractType.GetInterfaces().SelectMany(m => m.GetMethods());

            var methodsToProxy = declaredMethods.Union(inheritedMethods).Where(MethodIsNormalMethod);

            return methodsToProxy;
        }

        public bool MethodIsNormalMethod(MethodInfo method)
        {
            return !method.IsSpecialName;
        }
        
        public MethodBuilder BuildMethodProxy(TypeBuilder typeBuilder, MethodInfo contractMethod)
        {
            Type[] paramTypes = GetMethodParameterTypes(contractMethod);
            MethodBuilder methodBuilder = CreateProxyMethodBuilder(contractMethod, typeBuilder, paramTypes);

            ILGenerator il = methodBuilder.GetILGenerator();

            il.DeclareLocal(typeof(object[]));
            il.DeclareLocal(typeof(Type[]));

            il.Emit(OpCodes.Ldc_I4, paramTypes.Length);
            il.Emit(OpCodes.Newarr, typeof(object));
            il.Emit(OpCodes.Stloc, 0);

            for (int i = 0; i < paramTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldloc, 0);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldarg, i + 1);
                il.Emit(OpCodes.Stelem_Ref);
            }

            il.Emit(OpCodes.Ldloc, 0);
            il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeArray", BindingFlags.Static | BindingFlags.Public));
            il.Emit(OpCodes.Stloc, 1);

            il.Emit(OpCodes.Ldarg, 0); // this
            il.Emit(OpCodes.Ldarg, 0); // this

            var realSubjectField = typeBuilder.BaseType.GetField("_realSubject", BindingFlags.NonPublic | BindingFlags.Instance);

            il.Emit(OpCodes.Ldfld, realSubjectField);
            il.Emit(OpCodes.Callvirt, typeof(object).GetMethod("GetType"));

            il.Emit(OpCodes.Ldstr, contractMethod.Name);
            il.Emit(OpCodes.Ldloc, 1);

            il.Emit(OpCodes.Callvirt, typeof(Type).GetMethod("GetMethod", new[] { typeof(string), typeof(Type[]) }));

            il.Emit(OpCodes.Ldloc, 0);
            il.Emit(OpCodes.Call, typeof(InvocationProxy).GetMethod("Execute", BindingFlags.NonPublic | BindingFlags.Instance));

            if (methodBuilder.ReturnType == typeof(void)) il.Emit(OpCodes.Pop); // remove return value of Execute if void method

            il.Emit(OpCodes.Ret);

            return methodBuilder;
        }

        private MethodBuilder CreateProxyMethodBuilder(MethodInfo contractMethod, TypeBuilder typeBuilder, Type[] paramTypes)
        {
            return typeBuilder.DefineMethod(contractMethod.Name,
                                            MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot |
                                            MethodAttributes.Virtual | MethodAttributes.Final, contractMethod.ReturnType, paramTypes);
        }

        public static Type[] GetMethodParameterTypes(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();

            Type[] paramTypes = new Type[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                paramTypes[i] = parameters[i].ParameterType;
            }
            return paramTypes;
        }
    }
}