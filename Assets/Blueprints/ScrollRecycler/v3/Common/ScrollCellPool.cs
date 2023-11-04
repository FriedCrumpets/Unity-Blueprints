using System.Collections;
using System.Collections.Generic;
using Blueprints.Utility.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Blueprints.Scroller
{
    public class ScrollCellPool : IEnumerable<RectTransform>
    {
        public RectTransform this[System.Index index]
            => _active[index];
        
        public int ActiveCount 
            => _active.Count;

        public int InactiveCount
            => _inactive.Count;
        
        public int MaxCells { get; private set; }
        
        private readonly IList<RectTransform> _inactive = new List<RectTransform>();
        private readonly IList<RectTransform> _active = new List<RectTransform>();

        public RectTransform GetInactive(int index)
            => index < _inactive.Count ? _inactive[index] : default;
        
        public void Init(GameObject template, RectTransform container, int maxCells)
        {
            MaxCells = maxCells;
            
            for (var i = 0; i < maxCells; i++)
            {
                var cell = Object.Instantiate(template, container, false);
                if(cell.transform is RectTransform transform)
                {
                    transform.SetTopLeftAnchor();
                    _inactive.Add(transform);
                    cell.SetActive(false);
                }
                else
                    Object.Destroy(cell);
            }
        }

        public RectTransform ActivateCell()
        {
            var cell = PopTopCell(_inactive);
            _active.Add(cell);
            cell.gameObject.SetActive(true);
            return cell;
        }

        public IList<RectTransform> DeactivateAllCells()
        {
            var count = ActiveCount;
            var cells = new List<RectTransform>();
            
            for(var i = 0; i < count; i++)
                cells.Add(DeactivateCell());
            
            return cells;
        }

        public RectTransform DeactivateCell()
        {
            var cell = PopBottomCell(_active);
            _inactive.Add(cell);
            cell.gameObject.SetActive(false);
            return cell;
        }

        public RectTransform PopTopCell()
            => PopTopCell(_active);
        
        public RectTransform PopBottomCell()
            => PopBottomCell(_active);
        
        public void PushCellToTop(RectTransform cell)
            => PushCellToTop(_active, cell);
        
        public void PushCellToBottom(RectTransform cell)
            => PushCellToBottom(_active, cell);

        public RectTransform GetTopCell()
            => GetTopCell(_active);

        public RectTransform GetBottomCell()
            => GetBottomCell(_active);
        
        private static RectTransform GetTopCell(IList<RectTransform> pool)
            => pool[0].transform as RectTransform;

        private static RectTransform GetBottomCell(IList<RectTransform> pool)
            => pool[^1].transform as RectTransform;

        private static RectTransform PopTopCell(IList<RectTransform> pool)
        {
            var cell = pool[0];
            pool.Remove(cell);
            return cell;
        }
        
        private static RectTransform PopBottomCell(IList<RectTransform> pool)
        {
            var cell = pool[^1];
            pool.Remove(cell);
            return cell;
        }

        private static void PushCellToTop(IList<RectTransform> pool, RectTransform cell)
            => pool.Insert(0, cell);

        private static void PushCellToBottom(IList<RectTransform> pool, RectTransform cell)
            => pool.Add(cell);
        
        public IEnumerable<RectTransform> GetActive()
        {
            using var enumerator = _active.GetEnumerator();
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }
        
        public IEnumerable<RectTransform> GetInactive()
        {
            using var enumerator = _inactive.GetEnumerator();
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }

        public IEnumerator<RectTransform> GetEnumerator()
            => _active.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}