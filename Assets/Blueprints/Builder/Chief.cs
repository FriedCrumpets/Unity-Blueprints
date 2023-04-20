using System.Collections.Generic;
using System.Linq;

namespace Blueprints
{
    public class Chief<T>
    {
        private Queue<IBuild<T>> Builders { get; }

        public Chief()
        {
            Builders = new();
        }

        public Chief(IEnumerable<IBuild<T>> builders)
        {
            Builders = new Queue<IBuild<T>>();
            
            foreach (var builder in builders)
            {
                Buffer(builder);    
            }
        }

        public void Clear()
            => Builders.Clear();

        public void Buffer(IBuild<T> builder)
            => Builders.Enqueue(new Director<T>(builder));

        public T Build()
            => Builders.Dequeue().Build();

        public T Build(IBuild<T> builder)
            => builder.Build();
        
        public IEnumerable<T> BuildAll()
        {
            var built 
                = Builders.Select(director => director.Build());

            Builders.Clear();
            return built;
        }
    }
}