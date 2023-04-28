using Blueprints.Boot;
using Blueprints.ServiceLocator;
using Blueprints.Utility;
using UnityEngine;

namespace Blueprints.Facade.Service
{
    // not our component, but a big boi coded by some butthead
    public class ExampleMonolith : MonoBehaviour
    {
        public string Example { get; set; }
    }
    
    // The service that contains ONLY what we want to interface with
    public interface IExample : IService
    {
        string Example { get; set; }
    }
    
    // The Facade that mirrors exactly what is on the interface with the Monolith component
    public class ExampleFacade : IExample
    {
        private readonly ExampleMonolith _monolith;

        public ExampleFacade(ExampleMonolith monolith)
        {
            _monolith = monolith;
        }

        public string Example
        {
            get => _monolith.Example;
            set => _monolith.Example = value;
        }
    }

    // We can edit this at any time through the service provider, it's set up at the boot of the project
    public class ExampleDecorator : IExample
    {
        private string _example;
        
        private IExample _exampleFacade;

        public ExampleDecorator(IExample example = null)
        {
            ExampleFacade = example;
        }

        public IExample ExampleFacade
        {
            get => _exampleFacade;
            set
            {
                value.Example = _example;
                _example = value.Example;
                
                _exampleFacade = value;
            }
        }  

        public string Example
        {
            get => ExampleFacade?.Example ?? _example;
            set
            {
                if (ExampleFacade != null)
                {
                    ExampleFacade.Example = value;
                }

                _example = value;
            }
        }
    }
    
    // Example boot up script providing the services we need
    public sealed class ExampleBoot : TBoot
    {
        private ServiceHandler _services;

        private void Awake()
        {
            _services = new ServiceHandler();
            Boot();
        }

        protected override void CreateSingletons() { }

        protected override void ProvideServices()
        {
            _services.Provide(new ExampleDecorator());
        }
    }

    // And the controller on the object we've setup a facade for that provides the component to the wrapper
    public sealed class ExampleController : MonoBehaviour
    {
        private ExampleMonolith _monolith;

        private void Awake()
        {
            _monolith = GetComponent<ExampleMonolith>();
        }

        private void Start()
        {
            ((ExampleDecorator)ServiceHandler.Get<IExample>()).ExampleFacade = new ExampleFacade(_monolith);
        }
    }
    /*
     * How is this in any way better than singletons?
     *
     * A lot more going on; a lot harder to set up
     *
     * It just limits what you can touch through services and makes bug chasing much easier
     */
}