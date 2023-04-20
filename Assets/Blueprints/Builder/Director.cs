
namespace Blueprints
{
    public class Director<T> : IBuild<T>
    {
        private IBuild<T> _builder;
    
        public Director(IBuild<T> builder)
        {
            _builder = builder;
        }
    
        public void ChangeBuilder(IBuild<T> builder)
            => _builder = builder;
    
        public T Build()
            => _builder.Build();

        public T Destroy()
            => _builder.Destroy();
    }
}