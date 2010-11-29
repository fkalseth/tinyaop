using System;

namespace TinyAop.Tests.Mocks
{
    public class SubjectWithOnePropertyProxy : InvocationProxy, ISubjectWithOneProperty
    {
        private new readonly SubjectWithOneProperty _realSubject;

        public SubjectWithOnePropertyProxy(SubjectWithOneProperty realSubject, params IPointcut[] pointcuts) 
            : base(realSubject, pointcuts)
        {
            _realSubject = realSubject;
        }

        public object Property
        {
            get
            {
                object[] arguments = new object[] {};
                return Execute(_realSubject.GetType().GetProperty("Property").GetGetMethod(), arguments);
            }
            set
            {
                object[] arguments = new [] { value };
                Execute(_realSubject.GetType().GetProperty("Property").GetSetMethod(), arguments);
            }
        }

    }
}