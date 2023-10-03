using System;
using System.Collections;
using System.Collections.Generic;
using Blueprints.Utility.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Blueprints.Scroller
{
    [Serializable]
    public class ScrollCellPool : IEnumerable<RectTransform>
    {
        public RectTransform this[int index]
            => _active[index].transform as RectTransform;
        
        public int ActiveCount 
            => _active.Count;
        
        [field: SerializeField] public int MaxCells { get; private set; } = 10;
        
        [SerializeField] private GameObject template;
        
        private IList<RectTransform> _inactive = new List<RectTransform>();
        private IList<RectTransform> _active = new List<RectTransform>();

        public void Init(RectTransform container)
        {
            for (var i = 0; i < MaxCells; i++)
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
        
        public IEnumerable<RectTransform> GetEnumerable()
        {
            using var enumerator = _active.GetEnumerator();
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }

        public IEnumerator<RectTransform> GetEnumerator()
            => _active.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}