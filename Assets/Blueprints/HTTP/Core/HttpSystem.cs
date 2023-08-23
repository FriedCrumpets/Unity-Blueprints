using System.Collections;
using Blueprints.SystemFactory;
using UnityEngine;

namespace Blueprints.Http
{
    public class HttpSystem : MonoBehaviour, ISystem<Entity>
    {
        public void Init(Entity entity)
        {
            
        }

        private IEnumerator DequeueBuffer()
        {
            while (true)
            {
                
            }
        }

        public void Deinit()
        {
            
        }
    }
}