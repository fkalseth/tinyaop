using System.ComponentModel;

namespace TinyAop.Tests.Demo_scenarios.AutoNotifyPropertyChanged
{
    public interface IPerson : INotifyPropertyChanged
    {
        string Name { get; set; }
    }
}