using System.Collections.Generic;
using System.Reflection;

namespace TinyAop.Tests.Demo_scenarios.AutoNotifyPropertyChanged
{
    public class NotifyPropertyChangeAdvice : IAdvice, IPointcut
    {
        public void Execute(AdviceContext context)
        {
            var notifier = context.Proxy as IAutoNotifyPropertyChanged;
            notifier.NotifyPropertyChanged(context.Method.Name.Substring(4));
        }

        public IEnumerable<IAdvice> GetAdviceFor(MethodInfo joinpoint)
        {
            if(joinpoint.Name.StartsWith("set_"))
            {
                yield return this;
            }
        }
    }
}