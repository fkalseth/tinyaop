using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TinyAop.Tests.Mocks;

namespace TinyAop.Tests.Scenarios_for_ProxyFactory.Subjects_with_properties
{
    public class Specs_for_creating_proxy_with_properties : BDD<Specs_for_creating_proxy_with_properties>
    {
        private SubjectWithOneProperty _subject;
        private ISubjectWithOneProperty _proxy;

        readonly object _newValue = new object();
        private object _lastReadValue;

        [Test]
        public void Proxy_can_be_created()
        {
            Given.We_have_a_subject_with_one_property_to_intercept();
            When.We_create_the_proxy();
            Then.The_proxy_should_be_created_successfully();
        }

        [Test]
        public void Property_can_be_set()
        {
            Given.We_have_a_subject_with_one_property_to_intercept();
            When.We_create_the_proxy();
            And.We_set_the_property_via_proxy();
            Then.The_property_setter_is_invoked_on_subject();
            Then.The_property_is_set_on_the_subject();
        }

        private void The_property_setter_is_invoked_on_subject()
        {
            Assert.That(_subject.PropertySetterInvoked, Is.True);
        }

        [Test]
        public void Property_can_be_read()
        {
            Given.We_have_a_subject_with_one_property_to_intercept();
            When.We_create_the_proxy();
            And.We_get_the_property_via_proxy();
            Then.The_property_getter_is_invoked_on_subject();
            And.The_property_value_is_returned();
            
        }

        private void The_property_getter_is_invoked_on_subject()
        {
            Assert.That(_subject.PropertyGetterInvoked, Is.True);
        }

        private void The_property_value_is_returned()
        {
            Assert.That(_lastReadValue, Is.EqualTo(_subject.PropertyValue));
        }
        
        private void We_get_the_property_via_proxy()
        {
            _lastReadValue = _proxy.Property;
        }

        private void The_property_is_set_on_the_subject()
        {
            Assert.That(_subject.PropertyValue, Is.EqualTo(_newValue));
        }

        private void We_set_the_property_via_proxy()
        {
            _proxy.Property = _newValue;
        }

        private void The_proxy_should_be_created_successfully()
        {
            Assert.That(_proxy, Is.Not.Null);
        }

        private void We_create_the_proxy()
        {
            _proxy = new ProxyFactory().Create<ISubjectWithOneProperty>(_subject);
        }

        private void We_have_a_subject_with_one_property_to_intercept()
        {
            _subject = new SubjectWithOneProperty();
        }
    }
}