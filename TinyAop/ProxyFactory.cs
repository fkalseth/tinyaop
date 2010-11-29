using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace TinyAop
{
    public class ProxyFactory
    {
        private AssemblyBuilder _assemblyBuilder;
        
        private const string _assemblyFileName = "__aop_gen.dll";
        private const string _assemblyName = "__aop_gen";


        readonly List<IProxyImplementationStrategy> _buildStrategies = new List<IProxyImplementationStrategy>
        {
            new ProxyConstructorImplementationStrategy(),
            new ProxyMethodImplementationStrategy(),
            new ProxyPropertyImplementationStrategy(),
            new ProxyEventImplementationStrategy()
        };

        public void AddBuildStrategy(IProxyImplementationStrategy strategy)
        {
            _buildStrategies.Insert(0, strategy);
        }

        public TContract Create<TContract>(TContract subject, params IPointcut[] pointcuts) 
            where TContract : class
        {
            var proxy = CreateProxy(typeof(TContract), subject, pointcuts);
            return proxy as TContract;
        }

        private object CreateProxy(Type type, object subject, IPointcut[] pointcuts)
        {
            Type proxyType = CreateProxyType(type);
            
            _assemblyBuilder.Save(_assemblyFileName);
            
            var proxy = InstantiateProxy(subject, proxyType, pointcuts);
            return proxy;
        }

        private object InstantiateProxy(object subject, Type proxyType, IPointcut[] pointcuts)
        {
            var instanceOfProxy = Activator.CreateInstance(proxyType, subject, pointcuts);
            return instanceOfProxy;
        }

        private Type CreateProxyType(Type type)
        {
            TypeBuilder typeBuilder = CreateTypeBuilder(type);
            
            foreach(var buildStrategy in _buildStrategies)
            {
                buildStrategy.Build(typeBuilder, type);
            }
            
            var proxyType = typeBuilder.CreateType();
            return proxyType;
        }
        
        private TypeBuilder CreateTypeBuilder(Type type)
        {
            var assemblyName = new AssemblyName(_assemblyName);
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            
            ModuleBuilder moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name, _assemblyFileName);

            TypeBuilder typeBuilder = moduleBuilder.DefineType(
                type.Name.Substring(1) + "Proxy",
                TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable, 
                typeof(InvocationProxy));

            typeBuilder.AddInterfaceImplementation(type);

            return typeBuilder;
        }
    }
}