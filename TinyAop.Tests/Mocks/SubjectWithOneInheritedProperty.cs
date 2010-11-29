namespace TinyAop.Tests.Mocks
{
    public class SubjectWithOneInheritedProperty : ISubjectWithOneInheritedProperty
    {
        public object PropertyValue = new object();

        public object Property
        {
            get
            {
                PropertyGetterInvoked = true;
                return PropertyValue;
            }
            set
            {
                PropertySetterInvoked = true;
                PropertyValue = value;
            }
        }

        public bool PropertyGetterInvoked;
        public bool PropertySetterInvoked;
    }
}