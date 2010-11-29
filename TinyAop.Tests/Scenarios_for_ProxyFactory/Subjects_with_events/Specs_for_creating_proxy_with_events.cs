using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TinyAop.Tests.Mocks;

namespace TinyAop.Tests.Scenarios_for_ProxyFactory.Subjects_with_events
{
    [TestFixture]
    public class Specs_for_creating_proxy_with_events : BDD<Specs_for_creating_proxy_with_events>
    {
        private SubjectWithEvent _subject;
        private ISubjectWithEvent _proxy;
        private EventHandler _eventHandler;

        [Test]
        public void Proxy_can_be_created()
        {
            Given.We_have_a_subject_with_an_event();
            When.We_create_a_proxy();
            Then.The_proxy_is_created_successfully();
        }

        [Test]
        public void Event_handler_can_be_attached()
        {
            Given.We_have_a_subject_with_an_event();
            When.We_create_a_proxy();
            And.We_attach_an_event_handler();
            Then.The_handler_is_attached_to_event_on_subject();
        }

        [Test]
        public void Event_handler_can_be_removed()
        {
            Given.We_have_a_subject_with_an_event();
            When.We_create_a_proxy();
            And.We_attach_an_event_handler();
            And.We_remove_an_event_handler();
            Then.The_handler_is_no_longer_attached_to_event_on_subject();
        }

        private void We_remove_an_event_handler()
        {
            _proxy.Event -= _eventHandler;
        }

        private void The_handler_is_no_longer_attached_to_event_on_subject()
        {
            Assert.That(_subject.Handlers.Count(h => h == _eventHandler), Is.EqualTo(0));
        }

        private void The_handler_is_attached_to_event_on_subject()
        {
            Assert.That(_subject.Handlers.Count(h => h == _eventHandler), Is.EqualTo(1));
        }


        private void We_attach_an_event_handler()
        {
            _eventHandler = new EventHandler((o, h) => { });

            _proxy.Event += _eventHandler;
        }

        private void The_proxy_is_created_successfully()
        {
            Assert.That(_proxy, Is.Not.Null);
        }

        private void We_create_a_proxy()
        {
            _proxy = new ProxyFactory().Create<ISubjectWithEvent>(_subject);
        }

        private void We_have_a_subject_with_an_event()
        {
            _subject = new SubjectWithEvent();
        }
    }
}