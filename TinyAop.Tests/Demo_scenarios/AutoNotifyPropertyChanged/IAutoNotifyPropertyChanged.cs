using System.ComponentModel;

namespace TinyAop.Tests.Demo_scenarios.AutoNotifyPropertyChanged
{
    public interface IAutoNotifyPropertyChanged : INotifyPropertyChanged
    {
        void NotifyPropertyChanged(string propertyName);
    }
}