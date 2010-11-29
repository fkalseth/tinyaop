using System;
using System.Collections.Generic;
using System.Reflection;

namespace TinyAop
{
    public interface IPointcut
    {
        IEnumerable<IAdvice> GetAdviceFor(MethodInfo joinpoint);
    }
}