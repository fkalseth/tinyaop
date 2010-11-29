using System;
using System.Reflection;
using System.Reflection.Emit;

namespace TinyAop
{
    public interface IProxyImplementationStrategy
    {
        void Build(TypeBuilder builder, Type contractType);
    }
}