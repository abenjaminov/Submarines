using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MStudios.Grid
{
    public class Grid2D<T> : IGrid2D
    {
        private readonly List<GridObject2D<T>> _gridObjectInstances = new List<GridObject2D<T>>();
        private readonly List<Vector2Int> _occupiedCells = new List<Vector2Int>();

        public readonly int rows;
        public readonly int columns;

        public readonly Vector2 gridPosition;
        private IGridVisual<T> _visualization;
        
        public Grid2D(Vector2 position, int rows, int columns, IGridVisual<T> visualization)
        {
            _visualization = visualization;
            gridPosition = position;
            this.rows = rows;
            this.columns = columns;
        }

        public bool IsCellEmpty(Vector2 worldPosition)
        {
            return !_occupiedCells.Contains(ClampToGridInLocalPositions(worldPosition));
        }

        public T GetValueAt(Vector2 worldPosition)
        {
            var gridObject = GetObjectAt(worldPosition);

            if (gridObject == null)
            {
                return default(T);
            }
            else
            {
                return gridObject.GetValueAt(ClampToGridInLocalPositions(worldPosition));
            }
        }

        private GridObject2D<T> GetObjectAt(Vector2 worldPosition)
        {
            var positionInGridReferenceFrame = ClampToGridInLocalPositions(worldPosition);

            var gridObject = _gridObjectInstances.FirstOrDefault(o => o.IsPositionOnObject(positionInGridReferenceFrame));
            return gridObject;
        }

        public bool CanPutDownObject(GridObject2DData objectData, Vector2 worldPosition)
        {
            var positionInGridReferenceFrame = ClampToGridInLocalPositions(worldPosition);
            foreach (var occupiedOffsetOnObject in objectData.occupiedOffsets)
            {
                var point = positionInGridReferenceFrame + occupiedOffsetOnObject;
                if (_gridObjectInstances.Any(o => o.IsPositionOnObject(point))) return false;
            }

            return true;
        }
        
        public void PutDownObject(GridObject2DData objectData, Vector2 worldPosition, T value = default(T))
        {
            var positionOnGrid = SnapToGridInLocal(worldPosition, objectData);
            var newGridObject = new GridObject2D<T>("Grid Object", objectData,value);
            newGridObject.SetLocalPosition(new Vector2Int(positionOnGrid.x, positionOnGrid.y));

            _occupiedCells.AddRange(newGridObject.gridReferenceFrameCellPositions);
            
            _gridObjectInstances.Add(newGridObject);
            
            _visualization.Refresh(this);
        }

        public void ClearGridObjectAt(Vector2Int worldPosition)
        {
            var positionOnGrid = ClampToGridInLocalPositions(worldPosition);
            var gridObject = _gridObjectInstances.FirstOrDefault(x => x.gridReferenceFrameCellPositions.Contains(positionOnGrid));

            if (gridObject == null) return;

            _occupiedCells.RemoveAll(x => gridObject.gridReferenceFrameCellPositions.Contains(x));
            _gridObjectInstances.Remove(gridObject);
            _visualization.Refresh(this);
        }

        private Vector2Int SnapToGridInLocal(Vector2 centerWorldPosition, GridObject2DData objectData)
        {
            int rightOffset = (int) -objectData.rightPartWidth;
            int leftOffset = (int) objectData.leftPartWidth;
            int topOffset = (int) -objectData.topPartHeight;
            int bottomOffset = (int) objectData.bottomPartHeight;
            
            return ClampToGridInLocalPositions(centerWorldPosition, leftOffset, rightOffset, bottomOffset, topOffset);
        }
        
        public Vector2 SnapToGridInWorld(Vector2 centerWorldPosition, GridObject2DData objectData)
        {
            int rightOffset = (int) -objectData.rightPartWidth;
            int leftOffset = (int) objectData.leftPartWidth;
            int topOffset = (int) -objectData.topPartHeight;
            int bottomOffset = (int) objectData.bottomPartHeight;

            return ClampToGridInWorldPositions(centerWorldPosition, leftOffset, rightOffset, bottomOffset, topOffset);
        }

        private Vector2 ClampToGridInWorldPositions(Vector2 centerWorldPosition)
        {
            return ClampToGridInWorldPositions(centerWorldPosition, 0, 0, 0, 0);
        }
        
        private Vector2 ClampToGridInWorldPositions(Vector2 centerWorldPosition, int leftOffset, int rightOffset, int bottomOffset, int topOffset)
        {
            var centerLocalPosition = centerWorldPosition - gridPosition;

            Vector2 localGridPosition = ClampToGridInLocalPositions(centerWorldPosition, leftOffset, rightOffset, bottomOffset, topOffset);

            return localGridPosition + gridPosition;
        }

        private Vector2Int ClampToGridInLocalPositions(Vector2 centerWorldPosition)
        {
            return ClampToGridInLocalPositions(centerWorldPosition, 0, 0, 0, 0);
        }
        
        private Vector2Int ClampToGridInLocalPositions(Vector2 centerWorldPosition, int leftOffset, int rightOffset, int bottomOffset, int topOffset)
        {
            var centerLocalPosition = centerWorldPosition - gridPosition;

            Vector2Int localGridPosition = new Vector2Int
            {
                x = Mathf.Clamp(Mathf.RoundToInt(centerLocalPosition.x), leftOffset,
                    GetGridMaxBoundForObject(columns, rightOffset)),
                y = Mathf.Clamp(Mathf.RoundToInt(centerLocalPosition.y), bottomOffset,
                    GetGridMaxBoundForObject(rows, topOffset))
            };
            return localGridPosition;
        }

        public void SetValue(T value, Vector2 worldPosition)
        {
            GridObject2D<T> objectToSetValue = GetObjectAt(worldPosition);

            objectToSetValue?.SetValueAt(ClampToGridInLocalPositions(worldPosition), value);
        }

        public List<GridObject2D<T>> GetObjectsOnGrid()
        {
            return _gridObjectInstances;
        }

        private GridValue<T>[,] InitializeEmptyGridValues()
        {
            GridValue<T>[,] gridValues = new GridValue<T>[rows,columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    gridValues[i,j] = new GridValue<T>()
                    {
                        position = new Vector2(j, i),
                        value = default(T)
                    };
                }
            }

            return gridValues;
        }

        private void SetNonEmptyValues(GridValue<T>[,] gridValues)
        {
            foreach (var objectInstance in _gridObjectInstances)
            {
                foreach (var gridReferenceFrameCellPosition in objectInstance.gridReferenceFrameCellPositions)
                {
                    gridValues[gridReferenceFrameCellPosition.y, gridReferenceFrameCellPosition.x].value =
                        objectInstance.GetValueAt(gridReferenceFrameCellPosition);
                    gridValues[gridReferenceFrameCellPosition.y, gridReferenceFrameCellPosition.x].visual =
                        objectInstance.visual;
                }
            }
        }

        private int GetGridMaxBoundForObject(int targetGridAxesSize, int objectOffset)
        {
            return targetGridAxesSize + objectOffset - 1;
        }
    }
}