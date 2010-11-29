using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace TinyAop.Tests.Demo_scenarios.AutoNotifyPropertyChanged
{
    public class AutoNotifyPropertyChangedImplementationStrategy : IProxyImplementationStrategy
    {
        public void Build(TypeBuilder builder, Type contractType)
        {
            builder.AddInterfaceImplementation(typeof(IAutoNotifyPropertyChanged));
            
            FieldBuilder addPropertyChangedField = builder.DefineField("PropertyChanged", typeof (PropertyChangingEventHandler), FieldAttributes.Private);

            MethodBuilder addMethod = DefineAddOnMethod(builder, addPropertyChangedField);
            MethodBuilder removeMethod = DefineRemoveOnMethod(builder, addPropertyChangedField);
            MethodBuilder notifyPropertyChangedMethod = DefineRaiseMethod(builder, addPropertyChangedField);
            
            EventBuilder pcevent = builder.DefineEvent("PropertyChanged", EventAttributes.None, typeof(PropertyChangedEventHandler));
            pcevent.SetRaiseMethod(notifyPropertyChangedMethod);
            pcevent.SetAddOnMethod(addMethod);
            pcevent.SetRemoveOnMethod(removeMethod);
        }

        private MethodBuilder DefineRaiseMethod(TypeBuilder builder, FieldBuilder addPropertyChangedField)
        {
            var notifyPropertyChangedMethod = builder.DefineMethod("NotifyPropertyChanged", MethodAttributes.Public | MethodAttributes.Virtual, typeof(void),
                                                                   new [] {typeof (string)});

            var il = notifyPropertyChangedMethod.GetILGenerator();

            il.DeclareLocal(typeof (bool));

            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ldarg_0);
            
            il.Emit(OpCodes.Ldfld, addPropertyChangedField);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);

            Label ifNullLabel = il.DefineLabel();
            
            il.Emit(OpCodes.Brtrue_S, ifNullLabel);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, addPropertyChangedField);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Newobj, typeof (PropertyChangedEventArgs).GetConstructors().First());
            il.Emit(OpCodes.Callvirt, typeof(PropertyChangedEventHandler).GetMethod("Invoke"));
            il.MarkLabel(ifNullLabel);
            il.Emit(OpCodes.Ret);
            return notifyPropertyChangedMethod;
        }

        private MethodBuilder DefineRemoveOnMethod(TypeBuilder builder, FieldBuilder addPropertyChangedField)
        {
            ILGenerator gen;
            MethodBuilder removeMethod = builder.DefineMethod(
                "remove_PropertyChanged", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName | 
                                          MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot,
                typeof(void), new [] { typeof(PropertyChangedEventHandler) });

            gen = removeMethod.GetILGenerator();
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldfld, addPropertyChangedField);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Call, typeof(Delegate).GetMethod("Remove", new Type[] { typeof(Delegate), typeof(Delegate) }));
            gen.Emit(OpCodes.Castclass, typeof(PropertyChangedEventHandler));
            gen.Emit(OpCodes.Stfld, addPropertyChangedField);
            gen.Emit(OpCodes.Ret);
            return removeMethod;
        }

        private MethodBuilder DefineAddOnMethod(TypeBuilder builder, FieldBuilder addPropertyChangedField)
        {
            MethodBuilder addMethod = builder.DefineMethod(
                "add_PropertyChanged", 
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName | MethodAttributes.Final | 
                MethodAttributes.HideBySig | MethodAttributes.NewSlot,  
                typeof(void), new [] { typeof(PropertyChangedEventHandler) });  
            
            ILGenerator gen = addMethod.GetILGenerator();  
            gen.Emit(OpCodes.Ldarg_0);  
            gen.Emit(OpCodes.Ldarg_0);  
            gen.Emit(OpCodes.Ldfld, addPropertyChangedField);  
            gen.Emit(OpCodes.Ldarg_1);  
            gen.Emit(OpCodes.Call, typeof(Delegate).GetMethod("Combine", new Type[] { typeof(Delegate), typeof(Delegate) })); 
            gen.Emit(OpCodes.Castclass, typeof(PropertyChangedEventHandler)); 
            gen.Emit(OpCodes.Stfld, addPropertyChangedField); 
            gen.Emit(OpCodes.Ret);
            return addMethod;
        }
    }
}