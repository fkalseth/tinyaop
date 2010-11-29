namespace TinyAop.Tests.Mocks
{
    public class SubjectWithReturningMethod : ISubjectIWithReturningMethod
    {
        public object ReturnValue = new object();

        public object Method()
        {
            return ReturnValue;
        }
    }
}