using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace TinyAop.Tests.Demo_scenarios.SimpleDemo
{
    public class Simple_advice_demo
    {
        [Test]
        public void Test()
        {
            var proxyFactory = new ProxyFactory();

            var advisedFoo = proxyFactory.Create<IFoo>(new Foo(), new BarAdvice());

            advisedFoo.Bar();
        }

    }

    public interface IFoo
    {
        void Bar();
    }

    public class Foo : IFoo
    {
        public void Bar()
        {
            Console.WriteLine("Foo.Bar() executing");
        }
    }

    public class BarAdvice : IAdvice, IPointcut
    {
        public void Execute(AdviceContext context)
        {
            Console.WriteLine("About to call method...");

            context.Proceed();

            Console.WriteLine("...method called!");
        }

        public IEnumerable<IAdvice> GetAdviceFor(MethodInfo joinpoint)
        {
            yield return this; // apply advice to everything
        }
    }
}