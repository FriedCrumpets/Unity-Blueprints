using System;
using System.Collections.Generic;
using Blueprints.Utility.Extensions;
using UnityEngine;
using UnityEngine.UI;

// the ui part of the recycler
namespace Blueprints.Scroller
{
    public class InfiniteScroll : MonoBehaviour
    {
        [SerializeField] private ScrollRect ScrollRect;
        [field: SerializeField] public float Threshhold { get; private set; } = .2f;
        [SerializeField] private ScrollCellPool pool;
        
        private ScrollRecycler Recycler;
        private Bounds _boundary;
        private Queue<Action> _uiUpdates;
        private Vector2 _previousAnchoredPosition;

        private void Awake()
        {
            _uiUpdates = new Queue<Action>();
            _boundary = CreateViewBoundary(ScrollRect.viewport);
            Recycler = new ScrollRecycler(ScrollRect, _boundary, pool);
            _previousAnchoredPosition = Vector2.zero;
            
            pool.Init(ScrollRect.content);
            for (var i = 0; i < pool.MaxCells; i++)
                pool.ActivateCell();
            
            ScrollRect.onValueChanged.AddListener(OnScroll);
            
            _uiUpdates.Enqueue(() => ScrollRect.content.SetTopLeftAnchor());
            _uiUpdates.Enqueue(Recycler.ForceCellAnchors);
            _uiUpdates.Enqueue(Recycler.UpdateCellAnchors);
            _uiUpdates.Enqueue(() => ScrollRect.content.sizeDelta = new Vector2(ScrollRect.content.sizeDelta.x, Recycler.SizeDelta.y));
        }
        
        private void OnScroll(Vector2 value)
        {
            var direction = ScrollRect.content.anchoredPosition - _previousAnchoredPosition;
            
            if(direction != Vector2.zero)
            {
                var update = Recycler.OnScroll(direction);
                if( update != default )
                    _uiUpdates.Enqueue(() => update.Invoke());
            }
            
            _previousAnchoredPosition = ScrollRect.content.anchoredPosition;
        }
        
        private Bounds CreateViewBoundary(RectTransform view)
        {
            var corners = new Vector3[4];
            view.GetWorldCorners(corners);
            var threshHold = Threshhold * (corners[2].y - corners[0].y);
            
            return new Bounds
            {
                min = new Vector3(corners[0].x, corners[0].y - threshHold),
                max = new Vector3(corners[2].x, corners[2].y + threshHold)
            };
        }

        private void LateUpdate()
        {
            if(_uiUpdates.TryDequeue(out var update))
                update?.Invoke();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            ScrollRect.onValueChanged.RemoveListener(OnScroll);
        }
    }
}
