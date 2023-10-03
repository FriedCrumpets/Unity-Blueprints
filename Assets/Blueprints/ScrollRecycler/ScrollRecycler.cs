using System;
using System.Linq;
using Bloktopia.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Blueprints.Scroller
{
    public class ScrollRecycler
    {
        public Vector2 SizeDelta
            => _pool.Aggregate(
                new Vector2(0, TopSpacing), (current, cell) 
                    => current + new Vector2(0, cell.sizeDelta.y + TopSpacing)
            );
        
        public float TopSpacing { get; set; } = 30f;
        public float LeftSpacing { get; set; } = 5f;
        
        private readonly ScrollRect _scrollRect;
        public readonly Bounds _boundary;
        private readonly ScrollCellPool _pool;
        
        private bool TopCellOutOfBounds
            => _pool.ActiveCount > 0 && _pool.GetTopCell().GetBottomLeftCorner().y > _boundary.max.y;

        private bool BottomCellOutOfBounds
            => _pool.ActiveCount > 0 && _pool.GetBottomCell().GetTopLeftCorner().y < _boundary.min.y;
        
        public ScrollRecycler(ScrollRect scrollRect, Bounds boundary, ScrollCellPool pool)
        {
            _scrollRect = scrollRect;
            _boundary = boundary;
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
        
        private void IncreaseContentSizeByCellHeight(RectTransform cell)
            => _scrollRect.content.sizeDelta += new Vector2(0, TopSpacing + cell.sizeDelta.y);
        
        private void DecreaseContentSizeByCellHeight(RectTransform cell)
            =>  _scrollRect.content.sizeDelta -= new Vector2(0, TopSpacing + cell.sizeDelta.y);
    }
}