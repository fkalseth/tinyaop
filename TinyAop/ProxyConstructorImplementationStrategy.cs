using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace TinyAop
{
    public class ProxyConstructorImplementationStrategy : IProxyImplementationStrategy
    {
        public void Build(TypeBuilder typeBuilder, Type contractType)
        {
            ConstructorBuilder constructor = typeBuilder.DefineConstructor(
                MethodAttributes.Public, CallingConventions.HasThis, new[] { contractType, typeof(IPointcut[]) });

            ILGenerator il = constructor.GetILGenerator();

            // call base constructor passing second parameter of this constructor as argument
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Call, typeof(InvocationProxy).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).First());
            
            il.Emit(OpCodes.Ret);
        }
    }
}