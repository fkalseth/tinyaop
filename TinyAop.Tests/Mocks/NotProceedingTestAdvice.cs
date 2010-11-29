namespace TinyAop.Tests.Mocks
{
    public class NotProceedingTestAdvice : ProceedingTestAdvice
    {
        public NotProceedingTestAdvice() : base(false){}
    }
}