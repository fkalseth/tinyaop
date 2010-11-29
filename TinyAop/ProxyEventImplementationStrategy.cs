using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace TinyAop
{
    public class ProxyEventImplementationStrategy : IProxyImplementationStrategy
    {
        public void Build(TypeBuilder typeBuilder, Type contractType)
        {
            foreach (var @event in GetEventsToProxy(contractType))
            {
                BuildPropertyProxy(typeBuilder, @event);
            }
        }

        private IEnumerable<EventInfo> GetEventsToProxy(Type contractType)
        {
            var declaredEvents = contractType.GetEvents();
            var inheritedEvents = contractType.GetInterfaces().SelectMany(i => i.GetEvents());

            var eventsToProxy = declaredEvents.Union(inheritedEvents);
            return eventsToProxy;
        }

        private void BuildPropertyProxy(TypeBuilder typeBuilder, EventInfo contractEvent)
        {
            var builder = CreateProxyEventBuilder(typeBuilder, contractEvent);

            BuildRemover(typeBuilder, contractEvent, builder);
            BuildAdder(typeBuilder, contractEvent, builder);
        }

        private void BuildAdder(TypeBuilder typeBuilder, EventInfo contractProperty, EventBuilder builder)
        {
            var addMethod = contractProperty.GetAddMethod();

            if (null != addMethod)
            {
                var addMethodBuilder = new ProxyMethodImplementationStrategy().BuildMethodProxy(typeBuilder, addMethod);
                builder.SetAddOnMethod(addMethodBuilder);
            }
        }

        private void BuildRemover(TypeBuilder typeBuilder, EventInfo contractProperty, EventBuilder builder)
        {
            var removeMethod = contractProperty.GetRemoveMethod();

            if (null != removeMethod)
            {
                var removeMethodBuilder = new ProxyMethodImplementationStrategy().BuildMethodProxy(typeBuilder, removeMethod);
                builder.SetRemoveOnMethod(removeMethodBuilder);
            }
        }

        private EventBuilder CreateProxyEventBuilder(TypeBuilder typeBuilder, EventInfo contractEvent)
        {
            var builder = typeBuilder.DefineEvent(contractEvent.Name, contractEvent.Attributes,
                                                  contractEvent.EventHandlerType);

            return builder;
        }
    }
}