namespace TinyAop
{
    public interface IAdvice
    {
        void Execute(AdviceContext context);
    }
}