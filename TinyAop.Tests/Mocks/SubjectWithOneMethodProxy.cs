using System;

namespace TinyAop.Tests.Mocks
{
    public class SubjectWithOneMethodProxy : InvocationProxy, ISubjectWithOneMethod
    {
        private new readonly SubjectWithOneMethod _realSubject;

        public SubjectWithOneMethodProxy(SubjectWithOneMethod realSubject, params IPointcut[] pointcuts)
            : base(realSubject, pointcuts)
        {
            _realSubject = realSubject;
        }

        public bool MethodInvoked
        { 
            get { return _realSubject.MethodInvoked; }
        }

        public void Method(string argument)
        {
            object[] arguments = new object[]{argument};
            Type[] argumentTypes = Type.GetTypeArray(arguments);

            Execute(_realSubject.GetType().GetMethod("Method", argumentTypes), arguments);
        }

        public void Method(string arg1, object arg2)
        {
            object[] arguments = new object[]{arg1, arg2};
            Type[] argumentTypes = Type.GetTypeArray(arguments);

            Execute(_realSubject.GetType().GetMethod("Method", argumentTypes), arguments);
            
        }

        public void EmptyMethod(){}
    }
}