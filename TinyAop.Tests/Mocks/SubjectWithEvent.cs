using System;
using System.Collections.Generic;

namespace TinyAop.Tests.Mocks
{
    public class SubjectWithEvent : ISubjectWithEvent
    {
        List<EventHandler> _handlers = new List<EventHandler>();

        public EventHandler[] Handlers
        {
            get
            {
                return _handlers.ToArray();
            }
        }

        private EventHandler _event;

        public event EventHandler Event
        {
            add 
            {
                _event += value;
                _handlers.Add(value);
            
            }
            remove 
            {
                _event -= value;
                _handlers.Remove(value);
            }
        }
    }
}