using System;
using System.Reflection;

namespace TinyAop
{
    public class AdviceContext
    {
        public AdviceContext(Action onProceed, object proxy, object target, MethodInfo info, object[] arguments)
        {
            Method = info;
            Arguments = arguments;
            Target = target;
            Proxy = proxy;
            _onProceed = onProceed;
        }

        private readonly Action _onProceed;

        public object[] Arguments { get; private set; }
        public object Target { get; private set; }
        public object Proxy { get; private set; }

        public MethodInfo Method { get; private set; }

        public object ReturnValue { get; set; }

        public void Proceed()
        {
            _onProceed();
        }
    }
}
