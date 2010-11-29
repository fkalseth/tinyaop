using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TinyAop.Tests.Mocks;

namespace TinyAop.Tests.Scenarios_for_ProxyFactory.Subjects_with_methods
{
    [TestFixture]
    public class Specs_for_creating_proxy : BDD<Specs_for_creating_proxy>
    {
        [Test]
        public void Proxy_can_be_created()
        {
            Given.We_have_a_subject_with_one_parameterless_method_to_intercept();
            When.We_create_the_proxy();
            Then.The_proxy_should_be_created_successfully();
        }

        [Test]
        public void Proxy_inherits_InvocationProxy()
        {
            Given.We_have_a_subject_with_one_parameterless_method_to_intercept();
            When.We_create_the_proxy();
            Then.The_proxy_should_inherit_InvocationProxy();
        }

        [Test]
        public void Proxy_method_invokes_method_on_subject()
        {
            Given.We_have_a_subject_with_one_parameterless_method_to_intercept();
            When.We_create_the_proxy();
            And.We_call_a_method_on_the_proxy();
            Then.The_method_on_the_real_subject_should_be_invoked();
        }
        
        [Test]
        public void Advice_is_executed_when_proxy_has_advice_and_method_is_invoked()
        {
            Given.We_have_a_subject_with_one_parameterless_method_to_intercept();
            When.We_create_the_proxy_with_advice();
            And.We_call_a_method_on_the_proxy();
            Then.The_advice_should_be_executed();
        }
    
        private void We_call_a_method_on_the_proxy()
        {
            _proxy.Method();
        }

        private void We_have_a_subject_with_one_parameterless_method_to_intercept()
        {
            _realSubject = new SubjectWithOneParameterlessMethod();
        }

        private void We_create_the_proxy()
        {
            _proxy = new ProxyFactory().Create<ISubjectWithOneParameterlessMethod>(_realSubject);
        }

        private  void We_create_the_proxy_with_advice()
        {
            _proceedingAdvice = new ProceedingTestAdvice();
            _proxy = new ProxyFactory().Create<ISubjectWithOneParameterlessMethod>(_realSubject, _proceedingAdvice);
        }

        private void The_proxy_should_inherit_InvocationProxy()
        {
            Assert.That(_proxy as InvocationProxy, Is.InstanceOfType(typeof(InvocationProxy)));
        }

        private void The_method_on_the_real_subject_should_be_invoked()
        {
            Assert.That(_realSubject.MethodInvoked, Is.True);
        }
        
        private void The_proxy_should_be_created_successfully()
        {
            Assert.That(_proxy, Is.Not.Null);
        }

        private void The_advice_should_be_executed()
        {
            Assert.That(_proceedingAdvice.AdviceExecuted, Is.True);
        }

        private ISubjectWithOneParameterlessMethod _proxy;
        private SubjectWithOneParameterlessMethod _realSubject;
        private ProceedingTestAdvice _proceedingAdvice;
    }
}