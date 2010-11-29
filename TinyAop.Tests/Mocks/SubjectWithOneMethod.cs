using System;

namespace TinyAop.Tests.Mocks
{
    public class SubjectWithOneMethod : ISubjectWithOneMethod
    {
        public bool MethodInvoked { get; set; }

        public string ArgumentPassedToMethod { get; set; }

        public void Method(string argument)
        {
            ArgumentPassedToMethod = argument;
            MethodInvoked = true;
        }
    }
}