using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace TinyAop.Tests.Mocks
{
    public class ProceedingTestAdvice : IAdvice, IPointcut
    {
        private readonly bool _proceed;
        
        public ProceedingTestAdvice() : this(true){}

        public ProceedingTestAdvice(bool proceed)
        {
            _proceed = proceed;
        }

        public bool AdviceExecuted;
        public DateTime ExecutedWhen;

        public void Execute(AdviceContext context)
        {
            AdviceExecuted = true;
            Thread.Sleep(1); // ensure at least 1 ms between advice executions so ExecutedWhen is always unique
            ExecutedWhen = DateTime.Now;
            
            if(_proceed) context.Proceed();
        }

        public IEnumerable<IAdvice> GetAdviceFor(MethodInfo joinpoint)
        {
            yield return this;
        }
    }
}
