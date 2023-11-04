using System;
using UnityEngine;

namespace Blueprints.Builder.Fluent
{
    // simple object creation. I'm not a fan of the idea of having the builder class tied in as a child of this class so there may be a better way here
    public class Implementation : MonoBehaviour
    {
        private void Start()
        {
            var example = new v1.Builder()
                .WithName("example")
                .WithHealth(100)
                .WithDamage(2)
                .WithSpeed(5f);
        }
    }
    
    public class v1
    {
        public string Name { get; private set; }
        public int Health { get; private set; }
        public float Speed { get; private set; }
        public int Damage { get; private set; }

        private v1() { }
        
        public class Builder
        {
            private string name = "example";
            private int health = 100;
            private float speed = 5f;
            private int damage = 20;
    
            public Builder WithName(string name)
            {
                this.name = name;
                return this;
            }
            
            public Builder WithHealth(int health)
            {
                this.health = health;
                return this;
            }
            
            public Builder WithSpeed(float speed)
            {
                this.speed = speed;
                return this;
            }
            
            public Builder WithDamage(int damage)
            {
                this.damage = damage;
                return this;
            }
    
            public v1 Build()
                => new v1
                {
                    Name = this.name,
                    Health = this.health,
                    Speed = this.speed,
                    Damage = this.damage
                };
        }
    }
}