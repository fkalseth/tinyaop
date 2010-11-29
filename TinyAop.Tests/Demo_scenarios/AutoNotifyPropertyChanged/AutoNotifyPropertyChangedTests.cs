using System;
using System.ComponentModel;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace TinyAop.Tests.Demo_scenarios.AutoNotifyPropertyChanged
{
    public class AutoNotifyPropertyChangedTests
    {
        private bool _eventRaised;

        [Test]
        public void PropertyChanged_is_raised_when_property_changed()
        {
            var proxyFactory = new ProxyFactory();
            proxyFactory.AddBuildStrategy(new AutoNotifyPropertyChangedImplementationStrategy());

            var employee = proxyFactory.Create<IPerson>(new Person(), new NotifyPropertyChangeAdvice());

            employee.PropertyChanged += model_PropertyChanged; 

            employee.Name = "Fredrik";

            Assert.That(_eventRaised, Is.True);
        }
    
        void model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName + " was changed!");
            _eventRaised = true;
        }
    }
}