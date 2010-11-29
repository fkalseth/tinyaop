namespace TinyAop.Tests.Mocks
{
    public class SubjectWithOneParameterlessMethod : ISubjectWithOneParameterlessMethod
    {
        public bool MethodInvoked { get; set; }

        public void Method()
        {
            MethodInvoked = true;
        }
    }
}