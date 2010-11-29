using System.ComponentModel;

namespace TinyAop.Tests.Demo_scenarios.AutoNotifyPropertyChanged
{
    public class Person : IPerson
    {
        public string Name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}