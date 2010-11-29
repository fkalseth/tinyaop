using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TinyAop.Tests.Mocks;

namespace TinyAop.Tests.Scenarios_for_ProxyFactory.Subjects_with_methods
{
    [TestFixture]
    public class Specs_for_creating_proxy_when_method_inherited : BDD<Specs_for_creating_proxy_when_method_inherited>
    {
        private SubjectWithInheritedMethod _subject;
        private ISubjectWithInheritedMethod _proxy;

        [Test]
        public void Proxy_can_be_created()
        {
            Given.We_have_a_subject_with_a_two_level_interface_hierarchy();
            When.We_create_the_proxy();
            Then.The_proxy_should_be_created_successfully();
        }

        [Test]
        public void Proxy_inherits_InvocationProxy()
        {
            Given.We_have_a_subject_with_a_two_level_interface_hierarchy();
            When.We_create_the_proxy();
            Then.The_proxy_should_inherit_InvocationProxy();
        }

        [Test]
        public void Proxy_method_invokes_method_on_subject()
        {
            Given.We_have_a_subject_with_a_two_level_interface_hierarchy();
            When.We_create_the_proxy();
            And.We_call_a_method_on_the_proxy();
            Then.The_method_on_the_real_subject_should_be_invoked();
        }

        private void We_call_a_method_on_the_proxy()
        {
            _proxy.Method("argument");
        }

        private void The_proxy_should_be_created_successfully()
        {
            Assert.That(_proxy, Is.Not.Null);
        }

        private void The_method_on_the_real_subject_should_be_invoked()
        {
            Assert.That(_subject.MethodInvoked, Is.True);
        }

        private void The_proxy_should_inherit_InvocationProxy()
        {
            Assert.That(_proxy, Is.InstanceOfType(typeof(InvocationProxy)));
        }

        private void We_create_the_proxy()
        {
            _proxy = new ProxyFactory().Create<ISubjectWithInheritedMethod>(_subject);
        }

        private void We_have_a_subject_with_a_two_level_interface_hierarchy()
        {
            _subject = new SubjectWithInheritedMethod();
        }
    }
}