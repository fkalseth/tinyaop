using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TinyAop.Tests.Mocks;

namespace TinyAop.Tests.Scenarios_for_InvocationProxy
{
    [TestFixture]
    public class Specs_for_InvocationProxy_with_multiple_advice : BDD<Specs_for_InvocationProxy_with_multiple_advice>
    {
        [Test]
        public void All_advice_is_executed_when_all_advice_proceeds()
        {
            Given.We_have_a_proxy_with_two_advice_that_proceeds();
            When.We_invoke_the_Method_on_the_proxy();
            Then.The_outer_advice_should_be_executed();
            And.The_inner_advice_should_be_executed();
        }

        [Test]
        public void Advice_is_executed_in_correct_order()
        {
            Given.We_have_a_proxy_with_two_advice_that_proceeds();
            When.We_invoke_the_Method_on_the_proxy();
            Then.The_advice_should_be_executed_in_the_order_it_was_added();
        }

        [Test]
        public void Method_on_real_subject_should_be_invoked_when_all_advice_proceeds()
        {
            Given.We_have_a_proxy_with_two_advice_that_proceeds();
            When.We_invoke_the_Method_on_the_proxy();
            Then.The_method_on_the_real_subject_should_be_invoked();
        }

        [Test]
        public void Second_advice_should_not_execute_if_first_advice_does_NOT_proceed()
        {
            Given.We_have_a_proxy_with_two_advice_and_first_does_NOT_proceed();
            When.We_invoke_the_Method_on_the_proxy();
            Then.The_outer_advice_should_be_executed();
            And.The_inner_advice_should_NOT_be_executed();
        }

        [Test]
        public void Method_on_real_subject_is_NOT_invoked_if_one_advice_does_NOT_proceed()
        {
            Given.We_have_a_proxy_with_two_advice_and_first_does_NOT_proceed();
            When.We_invoke_the_Method_on_the_proxy();
            Then.The_method_on_the_real_subject_should_NOT_invoked();
        }

        private void We_have_a_proxy_with_two_advice_that_proceeds()
        {
            _innerAdvice = new ProceedingTestAdvice();
            _outerAdvice = new ProceedingTestAdvice();
            _realSubject = new SubjectWithOneParameterlessMethod();
            _testProxy = new SubjectWithOneParameterlessMethodProxy(_realSubject, _innerAdvice, _outerAdvice);
        }
        
        private void We_have_a_proxy_with_two_advice_and_first_does_NOT_proceed()
        {
            _innerAdvice = new ProceedingTestAdvice();
            _outerAdvice = new NotProceedingTestAdvice();
            _realSubject = new SubjectWithOneParameterlessMethod();
            _testProxy = new SubjectWithOneParameterlessMethodProxy(_realSubject, _innerAdvice, _outerAdvice);

        }

        private void We_invoke_the_Method_on_the_proxy()
        {
            _testProxy.Method();
        }

        private void The_method_on_the_real_subject_should_NOT_invoked()
        {
            Assert.That(_realSubject.MethodInvoked, Is.False);
        }

        private void The_inner_advice_should_be_executed()
        {
            Assert.That(_innerAdvice.AdviceExecuted, Is.True);
        }

        private void The_inner_advice_should_NOT_be_executed()
        {
            Assert.That(_innerAdvice.AdviceExecuted, Is.False);
        }

        private void The_outer_advice_should_be_executed()
        {
            Assert.That(_outerAdvice.AdviceExecuted, Is.True);
        }

        private void The_advice_should_be_executed_in_the_order_it_was_added()
        {
            Assert.That(_innerAdvice.ExecutedWhen, Is.GreaterThan(_outerAdvice.ExecutedWhen));
        }

        private void The_method_on_the_real_subject_should_be_invoked()
        {
            Assert.That(_realSubject.MethodInvoked, Is.True);
        }

        private ProceedingTestAdvice _innerAdvice;
        private ProceedingTestAdvice _outerAdvice;
        private SubjectWithOneParameterlessMethod _realSubject;
        private SubjectWithOneParameterlessMethodProxy _testProxy;
    }
}