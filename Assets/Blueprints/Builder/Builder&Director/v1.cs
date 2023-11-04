using UnityEngine;

namespace Blueprints.Builder.Builder_Director
{
    public class ExampleDirector
    {
        public Example Construct(ExampleBuilder builder, int health)
        {
            builder.AddWeapon();
            builder.AddHealth(health);
            return builder.Build();
        }
    }

    public class ExampleBuilder
    {
        private Example example = new GameObject("Example").AddComponent<Example>();

        public void AddWeapon()
            => example.gameObject.AddComponent<ExampleWeapon>();

        public void AddHealth(int health)
            => example.gameObject.AddComponent<ExampleHealth>();
        public Example Build()
            => example;
    }

    public class ExampleWeapon : MonoBehaviour { }

    public class ExampleHealth : MonoBehaviour
    {
        public void Init(int health) { }
    }
}