using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TinyAop.Tests.Mocks;

namespace TinyAop.Tests.Scenarios_for_InvocationProxy
{
    [TestFixture]
    public class Specs_for_InvocationProxy : BDD<Specs_for_InvocationProxy>
    {
        private ProceedingTestAdvice _proceedingTestAdvice;
        private ISubjectWithOneParameterlessMethod _testProxy;
        private SubjectWithOneParameterlessMethod _testSubject;

        [Test]
        public void Real_method_should_be_executed_when_proxy_has_no_advice()
        {
            Given.We_have_a_proxy_with_no_advice();
            When.We_invoke_a_method_on_the_proxy();
            Then.The_method_on_the_real_subject_should_be_invoked();
        }

        [Test]
        public void Advice_should_be_executed_when_proxy_has_advice()
        {
            Given.We_have_a_proxy_with_advice_that_proceeds();
            When.We_invoke_a_method_on_the_proxy();
            Then.The_advice_should_be_executed();
        }

        [Test]
        public void Method_should_be_executed_when_proxy_has_advice_that_proceeds()
        {
            Given.We_have_a_proxy_with_advice_that_proceeds();
            When.We_invoke_a_method_on_the_proxy();
            Then.The_method_on_the_real_subject_should_be_invoked();
        }

        [Test]
        public void Method_should_not_be_executed_when_proxy_has_advice_that_does_NOT_proceed()
        {
            Given.We_have_a_proxy_with_advice_that_does_not_proceed();
            When.We_invoke_a_method_on_the_proxy();
            Then.The_method_on_the_real_subject_should_NOT_be_invoked();
        }
    
        // GIVENs

        private void We_have_a_proxy_with_advice_that_proceeds()
        {
            _proceedingTestAdvice = new ProceedingTestAdvice();
            _testSubject = new SubjectWithOneParameterlessMethod();
            _testProxy = new SubjectWithOneParameterlessMethodProxy(_testSubject, _proceedingTestAdvice);
        }

        private void We_have_a_proxy_with_no_advice()
        {
            _testSubject = new SubjectWithOneParameterlessMethod();
            _testProxy = new SubjectWithOneParameterlessMethodProxy(_testSubject, null);
        }

        private void We_have_a_proxy_with_advice_that_does_not_proceed()
        {
            _proceedingTestAdvice = new NotProceedingTestAdvice();
            _testSubject = new SubjectWithOneParameterlessMethod();
            _testProxy = new SubjectWithOneParameterlessMethodProxy(_testSubject, _proceedingTestAdvice);
        }

        // WHENs

        private void We_invoke_a_method_on_the_proxy()
        {
            _testProxy.Method();
        }

        /// THENs

        private void The_method_on_the_real_subject_should_be_invoked()
        {
            Assert.That(_testSubject.MethodInvoked, Is.True);
        }

        private void The_method_on_the_real_subject_should_NOT_be_invoked()
        {
            Assert.That(_testSubject.MethodInvoked, Is.False);
        }

        private void The_advice_should_be_executed()
        {
            Assert.That(_proceedingTestAdvice.AdviceExecuted, Is.True);
        }
    }
}