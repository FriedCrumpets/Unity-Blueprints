using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Blueprints.Scroller
{
    public class ScrollDataController
    {
        private readonly IScrollDataSource _dataSource;
        private readonly ScrollRecycler _recycler;
        private readonly ScrollCellPool _pool;
        private readonly IList<Cell> _cells;
        private IQueue _queue;

        public int MaxIndex => _dataSource.MaxIndex;
        
        private bool RoomToActivateCell 
            => _pool.ActiveCount < _pool.MaxCells && _dataSource.MaxIndex > _pool.ActiveCount - 1;

        private bool CellDeactivationRequired 
            => _dataSource.MaxIndex < _pool.ActiveCount - 1;

        public ScrollDataController(IScrollDataSource dataSource, ScrollRecycler recycler, ScrollCellPool pool)
        {
            _dataSource = dataSource;
            _recycler = recycler;
            _pool = pool;
            _cells = new List<Cell>();
        }

        public void Init<T>(IQueue queue, params object[] args) where T : Component, IScrollCell
        {
            _queue = queue;
            foreach (var cell in _pool.GetInactive())
            {
                var component = cell.gameObject.ForceComponent<T>();
                component.Init(args);
                _cells.Add(new Cell(component));
            }
        }
        
        public Cell LocateCell(Object transform)
            => _cells.FirstOrDefault(cell => cell.Transform == transform);

        public void Activate()
        {
            DeActivate();
            _dataSource.DataAdded += OnDataAdded;
            _dataSource.DataUpdated += OnDataUpdated;
            _dataSource.DataRemoved += OnDataRemoved;
            _dataSource.DataCleared += OnDataCleared;
            _recycler.CellRecycled += OnCellRecycled;
        }
        
        public void DeActivate()
        {
            _dataSource.DataAdded -= OnDataAdded;
            _dataSource.DataUpdated -= OnDataUpdated;
            _dataSource.DataRemoved -= OnDataRemoved;
            _dataSource.DataCleared -= OnDataCleared;
            _recycler.CellRecycled -= OnCellRecycled;
        }
        
        private void OnDataAdded(int index)
        {
            if( _pool.InactiveCount > 0 )
            {
                var transform = _pool.ActivateCell();
                var cell = LocateCell(transform);
                UpdateCellData(cell, index);
            }
        }
        
        private void OnDataUpdated(int index)
        {
            if( DataIndexIsDisplayed(index, out var cellIndex) )
                UpdateDisplayedCellsFromIndex(cellIndex, index);
            // if(DataIndexAboveTopIndex(index))
            //     UpdateDisplayedCellsFromIndex();
        }
        
        private void OnDataRemoved(int index)
        {
            if( DataIndexIsDisplayed(index, out var cellIndex) )
                UpdateDisplayedCellsFromIndex(cellIndex, index);

            if( CellDeactivationRequired )
                _pool.DeactivateCell();
        }
        
        private void OnDataCleared()
        {
            _pool.DeactivateAllCells();
            _queue.Enqueue(_recycler.RefreshContentSize);
        }

        private void OnCellRecycled(RectTransform transform, int difference)
        {
            var cell = LocateCell(transform);
            var index = cell.CurrentDataIndex + difference;
            
            if( _dataSource.TrySetCell(cell.Component, index) )
                cell.CurrentDataIndex = index;
        }

        private void UpdateDisplayedCellsFromIndex(int cellIndex, int index)
        {
            for(var i = cellIndex; i < _pool.ActiveCount; i++)
                UpdateCellData(_cells[i], index++);
        }
        
        private void UpdateCellData(Cell cell, int index)
        {
            _queue.Enqueue(() =>
            {
                if( _dataSource.TrySetCell(cell.Component, index) )
                {
                    cell.CurrentDataIndex = index;
                    _queue.Enqueue(_recycler.UpdateCellAnchors);
                    _queue.Enqueue(_recycler.RefreshContentSize);
                }
            });
        }
        
        private bool DataIndexIsDisplayed(int index, out int cellIndex)
        {
            cellIndex = LocateCellWithDataIndex(index);
            return cellIndex != -1;
        }

        private int LocateCellWithDataIndex(int index)
            => _cells.IndexOf(_cells.FirstOrDefault(cell => cell.CurrentDataIndex == index));
    }
}