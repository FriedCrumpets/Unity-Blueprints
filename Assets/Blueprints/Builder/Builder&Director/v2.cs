using System;
using UnityEngine;

namespace Blueprints.Builder.Builder_Director.v2
{
    public class Implementation : MonoBehaviour
    {
        [SerializeField]
        private ExampleData data;
        
        private ExampleDirector _director;
        
        private void Start()
        {
            _director = new ExampleDirector(new ExampleBuilder());

            Example example = _director.Construct(data);
        }
    }
    
    public interface IExampleBuilder
    {
        IHealthExampleBuilder AddWeapon();
    }

    public interface IHealthExampleBuilder
    {
        IFinallyExampleBuilder AddHealth(int health);
    }

    public interface IFinallyExampleBuilder
    {
        Example Build();
    }

    public class ExampleDirector
    {
        private readonly ExampleBuilder _exampleBuilder;
        public ExampleDirector(ExampleBuilder exampleBuilder)
        {
            _exampleBuilder = exampleBuilder;
        }

        public Example Construct(ExampleData data)
            => _exampleBuilder
                .AddWeapon()
                .AddHealth(5)
                .Build();
    }
    
    public class ExampleBuilder : IExampleBuilder, IHealthExampleBuilder, IFinallyExampleBuilder
    {
        private Example example = new GameObject("Example").AddComponent<Example>();

        public IHealthExampleBuilder AddWeapon()
        {
            example.gameObject.AddComponent<ExampleWeapon>();
            return this;
        }

        public IFinallyExampleBuilder AddHealth(int health)
        {
            example.gameObject.AddComponent<ExampleHealth>();
            return this;
        }

        public Example Build()
        {
            var built = example;
            example = new GameObject("Example").AddComponent<Example>();
        
            return built;
        }
    }
    
    [Serializable]
    public class ExampleData { }
}