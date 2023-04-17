using System.Collections.Generic;

namespace Observer
{
    public interface IObservable
    {
        IList<IObserve> Observers { get; set; }
        
        void Subscribe(IObserve observable);
        void UnSubscribe(IObserve observable);
        void Publish();
    }
}