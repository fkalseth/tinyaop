using System;
using System.Reflection;

namespace TinyAop
{
    public class InvokeTargetMethodAdvice : IAdvice
    {
        private readonly MethodInfo _method;

        public InvokeTargetMethodAdvice(MethodInfo method)
        {
            _method = method;
        }

        public void Execute(AdviceContext context)
        {
            context.ReturnValue = _method.Invoke(context.Target, context.Arguments);
        }
    }
}