using System;
using System.Collections.Generic;
using Blueprints.ScrollRecycler.v2.Common;
using Blueprints.Utility.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Blueprints.ScrollRecycler.v2
{
    public class InfiniteScroll : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private int maxCells = 10;
        [SerializeField] private GameObject template;
        [SerializeField] public float threshhold = .2f;
        [SerializeField] public float topSpacing = 30f;
        [SerializeField] public float leftSpacing = 5f;
        
        private Common.ScrollRecycler _recycler;
        private ScrollCellPool _pool;
        private Bounds _boundary;
        private Queue<Action> _uiUpdates;
        private Vector2 _previousAnchoredPosition;

        private void Awake()
        {
            _pool = new ScrollCellPool();
            _uiUpdates = new Queue<Action>();
            _recycler = new Common.ScrollRecycler(threshhold, topSpacing, leftSpacing);
            _previousAnchoredPosition = Vector2.zero;
            
            _recycler.Init(scrollRect, _pool);
            
            _pool.Init(template, scrollRect.content, maxCells);
            for (var i = 0; i < maxCells; i++)
                _pool.ActivateCell();
            
            scrollRect.onValueChanged.AddListener(OnScroll);
            
            _uiUpdates.Enqueue(() => scrollRect.content.SetTopLeftAnchor());
            _uiUpdates.Enqueue(_recycler.ForceCellAnchors);
            _uiUpdates.Enqueue(_recycler.UpdateCellAnchors);
            _uiUpdates.Enqueue(_recycler.RefreshContentSize);
        }
        
        private void OnScroll(Vector2 value)
        {
            var direction = scrollRect.content.anchoredPosition - _previousAnchoredPosition;
            
            if(direction != Vector2.zero)
            {
                var update = _recycler.OnScroll(direction);
                if( update != default )
                    _uiUpdates.Enqueue(() => update.Invoke());
            }
            
            _previousAnchoredPosition = scrollRect.content.anchoredPosition;
        }

        private void LateUpdate()
        {
            if(_uiUpdates.TryDequeue(out var update))
                update?.Invoke();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            scrollRect.onValueChanged.RemoveListener(OnScroll);
        }
    }
}
