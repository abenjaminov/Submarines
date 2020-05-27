using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DefaultNamespace;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace GridUtils
{
    public class GridObject2D<T>
    {
        private string _name;
        private Vector2 _position;
        public readonly List<Vector2> localCellpositions = new List<Vector2>();
        public readonly List<Vector2> gridSpaceCellPositions = new List<Vector2>();
        private GridObject2DData _data;
        private Vector2 cellDrawOffset;
        public T value;

        public GridObject2D(string name, GridObject2DData data) : this(name, data.occupiedOffsets, data.pivot)
        {
            this._data = data;
        }
        
        public GridObject2D(string name, List<Vector2Int> occupiedOffsets, Vector2 cellDrawOffset)
        {
            this._data = null;
            this.cellDrawOffset = cellDrawOffset;

            foreach (var offset in occupiedOffsets)
            {
                var newCell = new Vector2(_position.x + offset.x, _position.y + offset.y);
                localCellpositions.Add(newCell);
            }
        }

        public void SetLocalPosition(Vector2 gridSpacePosition)
        {
            _position = gridSpacePosition + this.cellDrawOffset;

            var offset = _position - gridSpacePosition;
            UpdateCellsOffset(offset);
        }

        private void UpdateCellsOffset(Vector2 offset)
        {
            foreach (var cell in localCellpositions)
            {
                cell.Set(cell.x + offset.x, cell.y + offset.y);
            }

            CacheGridSpaceCellPositions();
        }

        private void CacheGridSpaceCellPositions()
        {
            gridSpaceCellPositions.Clear();
            gridSpaceCellPositions.AddRange(localCellpositions.Select(cell => new Vector2(_position.x + cell.x, _position.y + cell.y)));
        }

        public bool HasLocalPosition(Vector2 localPosition)
        {
            return localCellpositions.Contains(localPosition);
        }
    }
}