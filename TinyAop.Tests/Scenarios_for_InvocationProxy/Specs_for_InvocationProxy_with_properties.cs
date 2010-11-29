using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TinyAop.Tests.Mocks;

namespace TinyAop.Tests.Scenarios_for_InvocationProxy
{
    public class Specs_for_InvocationProxy_with_properties : BDD<Specs_for_InvocationProxy_with_properties>
    {
        private SubjectWithOneProperty _realSubject;
        private SubjectWithOnePropertyProxy _testProxy;
        private object _result;

        [Test]
        public void Real_property_getter_should_be_executed_when_proxy_has_no_advice()
        {
            Given.We_have_a_proxy_with_no_advice();
            When.We_invoke_a_property_getter_on_proxy();
            Then.The_property_getter_on_the_real_subject_should_be_invoked();
        }

        [Test]
        public void Property_value_should_be_returned_when_property_getter_executed_on_proxy()
        {
            Given.We_have_a_proxy_with_no_advice();
            When.We_invoke_a_property_getter_on_proxy();
            Then.The_expected_property_value_is_returned();
        }

        [Test]
        public void Real_property_setter_should_be_executed_when_proxy_has_no_advice()
        {
            Given.We_have_a_proxy_with_no_advice();
            When.We_invoke_a_property_setter_on_proxy();
            Then.The_property_setter_on_the_real_subject_should_be_invoked();
        }

        [Test]
        public void Property_value_should_be_changed_when_property_setter_executed_on_proxy()
        {
            Given.We_have_a_proxy_with_no_advice();
            When.We_invoke_a_property_setter_on_proxy();
            Then.The_property_on_the_real_subject_is_set_to_the_expected_value();
        }

        private void The_property_on_the_real_subject_is_set_to_the_expected_value()
        {
            Assert.That(_realSubject.PropertyValue, Is.EqualTo(_newValue));
        }

        private void The_property_setter_on_the_real_subject_should_be_invoked()
        {
            Assert.That(_realSubject.PropertySetterInvoked, Is.True);
        }

        private object _newValue = new object();

        private void We_invoke_a_property_setter_on_proxy()
        {
            _testProxy.Property = _newValue;
        }

        private void The_expected_property_value_is_returned()
        {
            Assert.That(_result, Is.EqualTo(_realSubject.PropertyValue));
        }

        private void The_property_getter_on_the_real_subject_should_be_invoked()
        {
            Assert.That(_realSubject.PropertyGetterInvoked, Is.True);
        }

        private void We_invoke_a_property_getter_on_proxy()
        {
            _result = _testProxy.Property;
        }

        private void We_have_a_proxy_with_no_advice()
        {
            _realSubject = new SubjectWithOneProperty();
            _testProxy = new SubjectWithOnePropertyProxy(_realSubject);
        }
    }
}