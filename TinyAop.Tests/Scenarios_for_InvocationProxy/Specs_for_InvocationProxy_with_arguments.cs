using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TinyAop.Tests.Mocks;

namespace TinyAop.Tests.Scenarios_for_InvocationProxy
{
    [TestFixture]
    public class Specs_for_InvocationProxy_with_arguments : BDD<Specs_for_InvocationProxy_with_arguments>
    {
        private SubjectWithOneMethod _realSubject;
        private SubjectWithOneMethodProxy _proxy;

        [Test]
        public void Arguments_should_be_passed_to_real_method()
        {
            Given.We_have_a_proxy_with_method_argument();
            When.We_invoke_a_method_on_the_proxy();
            Then.The_method_on_the_real_subject_should_be_invoked_with_the_argument();
        }

        private void We_have_a_proxy_with_method_argument()
        {
            _realSubject = new SubjectWithOneMethod();
            _proxy = new SubjectWithOneMethodProxy(_realSubject);
        }


        private void We_invoke_a_method_on_the_proxy()
        {
            _proxy.Method(_argument);
        }

        private void The_method_on_the_real_subject_should_be_invoked_with_the_argument()
        {
            Assert.That(_realSubject.MethodInvoked, Is.True);
            Assert.That(_realSubject.ArgumentPassedToMethod, Is.EqualTo(_argument));
        }

        private const string _argument = "argument";
    }
}