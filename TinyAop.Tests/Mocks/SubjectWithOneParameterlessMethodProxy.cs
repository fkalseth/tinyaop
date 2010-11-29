namespace TinyAop.Tests.Mocks
{
    public class SubjectWithOneParameterlessMethodProxy : InvocationProxy, ISubjectWithOneParameterlessMethod
    {
        private new readonly SubjectWithOneParameterlessMethod _realSubject;

        public SubjectWithOneParameterlessMethodProxy(SubjectWithOneParameterlessMethod realSubject, params IPointcut[] pointcuts)
            : base(realSubject, pointcuts)
        {
            _realSubject = realSubject;
        }

        public bool MethodInvoked
        { 
            get { return _realSubject.MethodInvoked; }
        }

        public void Method()
        {
            Execute(_realSubject.GetType().GetMethod("Method"));
        }
    }
}