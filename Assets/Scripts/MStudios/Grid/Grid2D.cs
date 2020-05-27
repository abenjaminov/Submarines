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

        private readonly Vector2 _gridPosition;
        private IGridVisual<T> _visualization;
        
        public Grid2D(Vector2 position, int rows, int columns, IGridVisual<T> visualization)
        {
            _visualization = visualization;
            _gridPosition = position;
            this.rows = rows;
            this.columns = columns;
        }

        public bool IsCellEmpty(Vector2 worldPosition)
        {
            return !_occupiedCells.Contains(ClampToGridInLocalPositions(worldPosition));
        }

        public bool CanPutDownObject(GridObject2DData objectData, Vector2 worldPosition)
        {
            var positionInGridReferenceFrame = ClampToGridInLocalPositions(worldPosition);
            return _gridObjectInstances.All(o => !o.IsPositionOnObject(positionInGridReferenceFrame));
        }
        
        public void PutDownObject(GridObject2DData objectData, Vector2 worldPosition, T value = default(T))
        {
            var positionOnGrid = SnapToGridInLocal(worldPosition, objectData);
            var newGridObject = new GridObject2D<T>("Grid Object", objectData);
            newGridObject.SetLocalPosition(new Vector2Int(positionOnGrid.x, positionOnGrid.y));
            newGridObject.value = value;
            
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
            var centerLocalPosition = centerWorldPosition - _gridPosition;

            Vector2 localGridPosition = ClampToGridInLocalPositions(centerWorldPosition, leftOffset, rightOffset, bottomOffset, topOffset);

            return localGridPosition + _gridPosition;
        }

        private Vector2Int ClampToGridInLocalPositions(Vector2 centerWorldPosition)
        {
            return ClampToGridInLocalPositions(centerWorldPosition, 0, 0, 0, 0);
        }
        
        private Vector2Int ClampToGridInLocalPositions(Vector2 centerWorldPosition, int leftOffset, int rightOffset, int bottomOffset, int topOffset)
        {
            var centerLocalPosition = centerWorldPosition - _gridPosition;

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
            var positionOnGrid = ClampToGridInLocalPositions(worldPosition);
            GridObject2D<T> objectToSetValue;
            
            if (!_occupiedCells.Contains(positionOnGrid))
            {
                objectToSetValue = new GridObject2D<T>("New Object",new List<Vector2Int>() {new Vector2Int(0, 0)}, Vector2.zero);
                objectToSetValue.SetLocalPosition(positionOnGrid);
                _gridObjectInstances.Add(objectToSetValue);
                _occupiedCells.AddRange(objectToSetValue.gridReferenceFrameCellPositions);
            }
            else
            {
                objectToSetValue = _gridObjectInstances.First(x => x.HasLocalPosition(positionOnGrid));
            }

            objectToSetValue.value = value;
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
                        objectInstance.value;
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