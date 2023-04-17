using UnityEngine;

namespace Blueprints
{
    /// <summary>
    /// The Decorator pattern is better executed with Interfaces over classes
    /// </summary>
    public abstract class BaseClass
    {
        public abstract void DoSomething();
    }
    
    public class ConcreteClass : BaseClass
    {
        public override void DoSomething()
        {
            Debug.Log("Concrete Class");
        }
    }
    
    public abstract class DecoratorClass : BaseClass
    {
        public abstract BaseClass Decorated { get; set; }
    }
    
    public class ConcreteDecorator : DecoratorClass
    {
        public override BaseClass Decorated { get; set; }

        public override void DoSomething()
        {
            Decorated.DoSomething();
            Debug.Log("Decorated Class");
        }
    }
}