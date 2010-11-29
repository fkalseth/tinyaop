using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TinyAop.Tests.Mocks;

namespace TinyAop.Tests.Scenarios_for_ProxyFactory.Subjects_with_methods
{
    [TestFixture]
    public class Specs_for_creating_proxy_when_method_returns_an_object : BDD<Specs_for_creating_proxy_when_method_returns_an_object>
    {
        [Test]
        public void Proxy_can_be_created()
        {
            Given.We_have_a_subject_with_returning_method_to_intercept();
            When.We_create_a_proxy();
            Then.The_proxy_should_be_created_successfully();
        }

        [Test]
        public void Proxy_returns_expected_value()
        {
            Given.We_have_a_subject_with_returning_method_to_intercept();
            When.We_create_a_proxy();
            And.We_invoke_the_method();
            Then.The_method_should_return_the_expected_value();
        }

        private void The_method_should_return_the_expected_value()
        {
            Assert.That(_result, Is.EqualTo(_realSubject.ReturnValue));
        }

        private object _result;

        private void We_invoke_the_method()
        {
            _result = _proxy.Method();
        }

        private void The_proxy_should_be_created_successfully()
        {
            Assert.That(_proxy, Is.Not.Null);
        }

        private ISubjectIWithReturningMethod _proxy;

        private SubjectWithReturningMethod _realSubject;

        private void We_have_a_subject_with_returning_method_to_intercept()
        {
            _realSubject = new SubjectWithReturningMethod();
        }

        private void We_create_a_proxy()
        {
            _proxy = new ProxyFactory().Create<ISubjectIWithReturningMethod>(_realSubject);
        }
    }
}