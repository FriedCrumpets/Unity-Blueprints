using System.Collections;
using Blueprints.Utility.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Blueprints.Scroller
{
    public class InfiniteScroll : MonoBehaviour
    {
        [SerializeField] public int maxCells = 10;
        [SerializeField] public float threshhold = .2f;
        [SerializeField] public float topSpacing = 30f;
        [SerializeField] public float leftSpacing = 5f;
        
        private ScrollRect _scrollRect;
        private ScrollRecycler _recycler;
        private ScrollCellPool _pool;
        private ScrollDataController _data;
        private IQueue _queue;
        
        private Vector2 _previousAnchoredPosition;

        public void Init<T>(ScrollRect scrollRect, GameObject template, IScrollDataSource dataSource, params object[] args) where T : Component, IScrollCell
        {
            _scrollRect = scrollRect;
            _recycler = new ScrollRecycler(threshhold, topSpacing, leftSpacing);
            _data = new ScrollDataController(dataSource, _recycler, _pool);
            _data.Activate();
            
            _recycler.Init(_data, scrollRect, _pool);
            _pool.Init(template, scrollRect.content, maxCells);
            _data.Init<T>(_queue, args);
            
            scrollRect.onValueChanged.RemoveListener(OnScroll);
            scrollRect.onValueChanged.AddListener(OnScroll);
            
            _queue.Enqueue(() => scrollRect.content.SetTopLeftAnchor());
            _queue.Enqueue(_recycler.ForceCellAnchors);
            _queue.Enqueue(_recycler.UpdateCellAnchors);
            _queue.Enqueue(_recycler.RefreshContentSize);

            StartCoroutine(ExecuteQueue());
        }

        public void DeInit()
        {
            _data.DeActivate();
            _scrollRect.onValueChanged.RemoveListener(OnScroll);
            StopAllCoroutines();
        }
        
        private void Awake()
        {
            _pool = new ScrollCellPool();
            _queue = new UIUpdateQueue();
            _previousAnchoredPosition = Vector2.zero;
        }
        
        private void OnScroll(Vector2 value)
        {
            var direction = _scrollRect.content.anchoredPosition - _previousAnchoredPosition;
            
            if(direction != Vector2.zero)
            {
                var update = _recycler.OnScroll(direction);
                if( update != default )
                    _queue.Enqueue(() => update.Invoke());
            }
            
            _previousAnchoredPosition = _scrollRect.content.anchoredPosition;
        }

        private IEnumerator ExecuteQueue()
        {
            var waitforendofframe = new WaitForEndOfFrame();
            while (true)
            {
                yield return waitforendofframe;
                _queue.TryDequeue()?.Invoke();
                yield return null;
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            _scrollRect.onValueChanged.RemoveListener(OnScroll);
        }
    }

}
