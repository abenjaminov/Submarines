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
        private readonly List<Vector2Int> _objectReferenceFrameCellPositions = new List<Vector2Int>();
        public readonly List<Vector2Int> gridReferenceFrameCellPositions = new List<Vector2Int>();
        private readonly GridObject2DData _data;
        public Sprite visual => _data.visual;
        public T value;

        public GridObject2D(string name, GridObject2DData data) : this(name, data.occupiedOffsets, data.centerOffset)
        {
            this._data = data;
        }
        
        public GridObject2D(string name, List<Vector2Int> occupiedOffsets, Vector2 cellDrawOffset)
        {
            this._data = null;

            foreach (var offset in occupiedOffsets)
            {
                var newCell = new Vector2Int(gridReferenceFramePosition.x + offset.x, gridReferenceFramePosition.y + offset.y);
                _objectReferenceFrameCellPositions.Add(newCell);
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
            gridReferenceFrameCellPositions.AddRange(_objectReferenceFrameCellPositions.Select(cell => new Vector2Int(gridReferenceFramePosition.x + cell.x, gridReferenceFramePosition.y + cell.y)));
        }

        public bool HasLocalPosition(Vector2Int localPosition)
        {
            return _objectReferenceFrameCellPositions.Contains(localPosition);
        }

        public bool IsPositionOnObject(Vector2Int positionInGridReferenceFrame)
        {
            return this.gridReferenceFrameCellPositions.Contains(positionInGridReferenceFrame);
        }
    }
}