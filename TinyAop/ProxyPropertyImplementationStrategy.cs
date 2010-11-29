using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace TinyAop
{
    public class ProxyPropertyImplementationStrategy : IProxyImplementationStrategy
    {
        public void Build(TypeBuilder typeBuilder, Type contractType)
        {
            foreach (var property in GetPropertiesToProxy(contractType))
            {
                BuildPropertyProxy(typeBuilder, property);
            }
        }

        private IEnumerable<PropertyInfo> GetPropertiesToProxy(Type contractType)
        {
            var declaredProperties = contractType.GetProperties();
            var inheritedProperties = contractType.GetInterfaces().SelectMany(i => i.GetProperties());

            var propertiesToProxy = declaredProperties.Union(inheritedProperties);
            return propertiesToProxy;
        }

        private void BuildPropertyProxy(TypeBuilder typeBuilder, PropertyInfo contractProperty)
        {
            var builder = CreatePropertyProxyBuilder(typeBuilder, contractProperty);

            BuildGetter(typeBuilder, contractProperty, builder);
            BuildSetter(typeBuilder, contractProperty, builder);
        }

        private void BuildSetter(TypeBuilder typeBuilder, PropertyInfo contractProperty, PropertyBuilder builder)
        {
            var setMethod = contractProperty.GetSetMethod();

            if (null != setMethod)
            {
                var setMethodBuilder = new ProxyMethodImplementationStrategy().BuildMethodProxy(typeBuilder, setMethod);
                builder.SetSetMethod(setMethodBuilder);
            }
        }

        private void BuildGetter(TypeBuilder typeBuilder, PropertyInfo contractProperty, PropertyBuilder builder)
        {
            var getMethod = contractProperty.GetGetMethod();

            if(null != getMethod)
            {
                var getMethodBuilder = new ProxyMethodImplementationStrategy().BuildMethodProxy(typeBuilder, getMethod);
                builder.SetGetMethod(getMethodBuilder);
            }
        }

        private PropertyBuilder CreatePropertyProxyBuilder(TypeBuilder typeBuilder, PropertyInfo contractProperty)
        {
            var builder = typeBuilder.DefineProperty(contractProperty.Name, contractProperty.Attributes,
                                                     contractProperty.PropertyType, null);

            return builder;
        }
    }
}