using Blueprints.Components;

namespace Blueprints.Facade.Service
{
    public class SettingsComponent : StaticComponent<ExampleMonolith>, IExample
    {
        private string _example;
        
        string IExample.Example
        {
            get => Monolith != null ? Monolith.Example : _example;
            set
            {
                if (Monolith != null)
                    Monolith.Example = value;
                
                _example = value;
            }
        }
        
        private void Start()
        {
            Send<InitComponent>(component =>
            {
                component.example = this;
            });
        }
    }
    
    public class InitComponent : StaticComponent<ExampleMonolith>
    {
        private IExample _example;

        public IExample example
        {
            get => _example;
            set
            {
                Monolith.Example = value.Example;
                value.Example = Monolith.Example;
            }
        }
    }
}