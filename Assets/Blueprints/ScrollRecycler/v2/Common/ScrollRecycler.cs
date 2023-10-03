using System;
using System.Linq;
using Blueprints.Scroller;
using UnityEngine;
using UnityEngine.UI;

namespace Blueprints.ScrollRecycler.v2.Common
{
    public class ScrollRecycler
    {
        public Vector2 SizeDelta
            => _pool.Aggregate(
                new Vector2(0, TopSpacing), (current, cell) 
                    => current + new Vector2(0, cell.sizeDelta.y + TopSpacing)
            );
        
        public float Threshhold { get; }
        public float TopSpacing { get; }
        public float LeftSpacing { get; }

        
        private Bounds _boundary;
        private ScrollRect _scrollRect;
        private ScrollCellPool _pool;
        
        private bool TopCellOutOfBounds
            => _pool.ActiveCount > 0 && _pool.GetTopCell().GetBottomLeftCorner().y > _boundary.max.y;

        private bool BottomCellOutOfBounds
            => _pool.ActiveCount > 0 && _pool.GetBottomCell().GetTopLeftCorner().y < _boundary.min.y;

        public ScrollRecycler(float threshhold, float topSpacing, float leftSpacing)
        {
            Threshhold = threshhold;
            TopSpacing = topSpacing;
            LeftSpacing = leftSpacing;
        }
        
        public void Init(ScrollRect scrollRect, ScrollCellPool pool)
        {
            _scrollRect = scrollRect;
            _boundary = CreateViewBoundary(_scrollRect.viewport);
            _pool = pool;
        }
        
        public Func<RectTransform> OnScroll(Vector2 direction)
        {
            return direction.y switch
            {
                > 0 when TopCellOutOfBounds => RecycleFromTop,
                < 0 when BottomCellOutOfBounds => RecycleFromBottom,
                _ => default
            };
        }
        
        public void ForceCellAnchors()
        {
            foreach(var cell in _pool)
                if(cell.transform is RectTransform transform)
                    transform.SetTopLeftAnchor();
        }
        
        public void UpdateCellAnchors()
        {
            RectTransform previousCell = null;
            for (var i = 0; i < _pool.ActiveCount; i++)
            {
                _pool[i].anchoredPosition = previousCell == null 
                    ? new Vector2(LeftSpacing, -TopSpacing) 
                    : new Vector2(LeftSpacing, previousCell.anchoredPosition.y - previousCell.sizeDelta.y - TopSpacing);
                
                previousCell = _pool[i];
            }   
        }

        public void RefreshContentSize()
            => _scrollRect.content.sizeDelta = new Vector2(_scrollRect.content.sizeDelta.x, SizeDelta.y);

        private RectTransform RecycleFromTop()
        {
            var cell = _pool.PopTopCell();
            PushCellToBottom(cell);
            IncreaseContentSizeByCellHeight(cell);
            _pool.PushCellToBottom(cell);
            return cell;
        }

        private void PushCellToBottom(RectTransform cell)
        {
            cell.anchoredPosition = _pool.ActiveCount switch
            {
                > 0 => _pool.GetBottomCell().anchoredPosition - new Vector2(0, _pool.GetBottomCell().sizeDelta.y + TopSpacing),
                _ => new Vector2(LeftSpacing, -TopSpacing)
            };
        }

        private RectTransform RecycleFromBottom()
        {
            var cell = _pool.PopBottomCell();
            PushCellToTop(cell);
            DecreaseContentSizeByCellHeight(cell);
            _pool.PushCellToTop(cell);
            return cell;
        }
        
        private void PushCellToTop(RectTransform cell)
        {
            cell.anchoredPosition = _pool.ActiveCount switch
            {
                > 0 => _pool.GetTopCell().anchoredPosition + new Vector2(0, cell.sizeDelta.y + TopSpacing),
                _ => new Vector2(LeftSpacing, -TopSpacing)
            };
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
        
        private void IncreaseContentSizeByCellHeight(RectTransform cell)
            => _scrollRect.content.sizeDelta += new Vector2(0, TopSpacing + cell.sizeDelta.y);
        
        private void DecreaseContentSizeByCellHeight(RectTransform cell)
            =>  _scrollRect.content.sizeDelta -= new Vector2(0, TopSpacing + cell.sizeDelta.y);
    }
}