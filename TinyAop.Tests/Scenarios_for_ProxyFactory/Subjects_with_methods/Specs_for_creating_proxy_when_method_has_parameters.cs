using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TinyAop.Tests.Mocks;

namespace TinyAop.Tests.Scenarios_for_ProxyFactory.Subjects_with_methods
{
    [TestFixture]
    public class Specs_for_creating_proxy_when_method_has_parameters : BDD<Specs_for_creating_proxy_when_method_has_parameters>
    {
        [Test]
        public void Proxy_can_be_created()
        {
            Given.We_have_a_subject_with_one_method_to_intercept();
            When.We_create_a_proxy();
            Then.The_proxy_should_be_created_successfully();
        }

        [Test]
        public void Proxy_inherits_InvocationProxy()
        {
            Given.We_have_a_subject_with_one_method_to_intercept();
            When.We_create_a_proxy();
            Then.The_proxy_should_inherit_InvocationProxy();
        }

        [Test]
        public void Proxy_method_invokes_method_on_subject()
        {
            Given.We_have_a_subject_with_one_method_to_intercept();
            
            When.We_create_a_proxy();
            And.Then.We_call_the_Method_on_the_proxy();
            
            Then.The_method_on_the_real_subject_should_be_invoked();
            And.The_method_on_the_real_subject_should_be_passed_the_argument();
        }

        private void The_proxy_should_inherit_InvocationProxy()
        {
            Assert.That(_proxy as InvocationProxy, Is.InstanceOfType(typeof(InvocationProxy)));
        }

        private void We_call_the_Method_on_the_proxy()
        {
            _proxy.Method(_argument);
        }

        private void The_method_on_the_real_subject_should_be_invoked()
        {
            Assert.That(_realSubject.MethodInvoked, Is.True);
        }


        private void We_have_a_subject_with_one_method_to_intercept()
        {
            _realSubject = new SubjectWithOneMethod();
        }

        private void We_create_a_proxy()
        {
            _proxy = new ProxyFactory().Create<ISubjectWithOneMethod>(_realSubject);
        }

        private void The_proxy_should_be_created_successfully()
        {
            Assert.That(_proxy, Is.Not.Null);
        }

        private void The_method_on_the_real_subject_should_be_passed_the_argument()
        {
            Assert.That(_realSubject.ArgumentPassedToMethod, Is.EqualTo(_argument));
        }

        private const string _argument = "argument";

        private ISubjectWithOneMethod _proxy;

        private SubjectWithOneMethod _realSubject;
    }
}