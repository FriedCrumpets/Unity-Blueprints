namespace Observer
{
    public interface IObserve
    {
        void Observe(IObservable observable);

        void Execute();
    }
}