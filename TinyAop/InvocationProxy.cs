using System.Collections.Generic;
using System.Reflection;

namespace TinyAop
{
    public abstract class InvocationProxy
    {
        protected readonly object _realSubject;
        private readonly List<IPointcut> _pointcuts = new List<IPointcut>();

        protected InvocationProxy(object realSubject, params IPointcut[] pointcuts)
        {
            _realSubject = realSubject;
            if(null != pointcuts) _pointcuts.AddRange(pointcuts);
        }

        protected object Execute(MethodInfo method, params object[] arguments)
        {
            Stack<IAdvice> advice = BuildAdviceStackFor(method);
            AdviceContext adviceContext = BuildAdviceContextFor(method, advice, arguments);

            advice.Pop().Execute(adviceContext);
            return adviceContext.ReturnValue;
        }

        private Stack<IAdvice> BuildAdviceStackFor(MethodInfo method)
        {
            var advice = new Stack<IAdvice>();
            advice.Push(new InvokeTargetMethodAdvice(method));

            _pointcuts.ForEach(p => p.GetAdviceFor(method).ForEach(advice.Push));
            return advice;
        }

        private AdviceContext BuildAdviceContextFor(MethodInfo method, Stack<IAdvice> advice, object[] arguments)
        {
            AdviceContext adviceContext = null;
            adviceContext = new AdviceContext(() => advice.Pop().Execute(adviceContext), this, _realSubject, method, arguments);

            return adviceContext;
        }
    }
}