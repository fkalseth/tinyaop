using System;

namespace TinyAop.Tests.Mocks
{
    public interface ISubjectWithEvent
    {
        event EventHandler Event;
    }
}