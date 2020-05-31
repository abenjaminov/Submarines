using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace MStudios.Grid
{
    public class GridObject2D<T>
    {
        private string _name;
        public Vector2Int gridReferenceFramePosition;
        private readonly List<GridObjectCell<T>> _gridObjectCells = new List<GridObjectCell<T>>();
        public readonly List<Vector2Int> gridReferenceFrameCellPositions = new List<Vector2Int>();
        private readonly GridObject2DData _data;
        public Sprite visual => _data.visual;
        private readonly T _initialValue;

        public GridObject2D(string name, GridObject2DData data, T initialValue = default(T)) : this(name, data.occupiedOffsets, data.centerOffset, initialValue)
        {
            this._data = data;
        }

        private GridObject2D(string name, List<Vector2Int> occupiedOffsets, Vector2 cellDrawOffset, T initialValue = default(T))
        {
            this._data = null;
            _initialValue = initialValue;

            foreach (var offset in occupiedOffsets)
            {
                var newCellLocalPosition = new Vector2Int(gridReferenceFramePosition.x + offset.x, gridReferenceFramePosition.y + offset.y);
                _gridObjectCells.Add(new GridObjectCell<T>()
                {
                    value = _initialValue,
                    localPosition = newCellLocalPosition
                });
            }
        }

        public void SetLocalPosition(Vector2Int gridSpacePosition)
        {
            gridReferenceFramePosition = gridSpacePosition;
            
            CacheGridSpaceCellPositions();
        }

        private void CacheGridSpaceCellPositions()
        {
            gridReferenceFrameCellPositions.Clear();
            gridReferenceFrameCellPositions.AddRange(_gridObjectCells.Select(cell => 
                new Vector2Int(gridReferenceFramePosition.x + cell.localPosition.x, 
                    gridReferenceFramePosition.y + cell.localPosition.y)));
        }

        public bool HasLocalPosition(Vector2Int localPosition)
        {
            return _gridObjectCells.Any(x => x.localPosition == localPosition);
        }

        public bool IsPositionOnObject(Vector2Int positionInGridReferenceFrame)
        {
            return this.gridReferenceFrameCellPositions.Contains(positionInGridReferenceFrame);
        }

        public T GetValueAt(Vector2Int positionInGridReferenceFrame)
        {
            if (IsPositionOnObject(positionInGridReferenceFrame))
            {
                var localPosition = positionInGridReferenceFrame - gridReferenceFramePosition;
                var cell = _gridObjectCells.FirstOrDefault(x => x.localPosition == localPosition);

                if (cell != null)
                {
                    return cell.value;
                }
            }

            return default(T);
        }

        public void SetValueAt(Vector2Int positionInGridReferenceFrame, T value)
        {
            var localPosition = positionInGridReferenceFrame - gridReferenceFramePosition;
            var cell = _gridObjectCells.FirstOrDefault(x => x.localPosition == localPosition);
            
            if (cell != null)
            {
                cell.value = value;
            }
        }

        public bool HasCellWithValue(T value)
        {
            return _gridObjectCells.Any(x => x.value.Equals(value));
        }

        public Vector2Int GetRandomLocalCellPositionWithValue(T value)
        {
            var relevantCellPositions = GetAllLocalPositionsWithValue(value);
            return relevantCellPositions[Random.Range(0, relevantCellPositions.Count)];
        }

        public List<Vector2Int> GetAllLocalPositionsWithValue(T value)
        {
            return _gridObjectCells.Where(x => x.value.Equals(value)).Select(x => x.localPosition).ToList();
        }
    }
}