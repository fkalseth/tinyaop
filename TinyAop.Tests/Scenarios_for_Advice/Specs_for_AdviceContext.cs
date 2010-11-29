using System;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace TinyAop.Tests.Scenarios_for_Advice
{
    [TestFixture]
    public class Specs_for_AdviceContext : BDD<Specs_for_AdviceContext>
    {
        private bool _proceedActionInvoked;
        private AdviceContext _context;

        [Test]
        public void Proceed_action_is_invoked()
        {
            Given.We_have_an_AdviceContext();
            When.The_Proceed_method_is_invoked();
            Then.The_proceed_action_should_be_executed();
        }

        private void We_have_an_AdviceContext()
        {
            _context = new AdviceContext(() => _proceedActionInvoked = true, null, null, null, null);
        }

        private void The_Proceed_method_is_invoked()
        {
            _context.Proceed();
        }

        private void The_proceed_action_should_be_executed()
        {
            Assert.That(_proceedActionInvoked, Is.True);
        }
    }
}