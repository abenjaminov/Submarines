using System.Collections.Generic;
using MStudios.Events.GameEvents;
using UnityEngine;

namespace MStudios.Grid
{
    public class GridVisual<T> : MonoBehaviour, IGridVisual<T>
    {
        [Header("Debug")]
        [SerializeField] private bool drawDebugGrid;
        
        [Header("Grid Data")]
        [SerializeField] protected int rows;
        [SerializeField] protected int columns;
        [Space]
        [SerializeField] protected List<GridObject2DData> gridObjectsData;
        

        [SerializeField]
        private Position2DGameEvent gridLocationSelectedEvent;
        
        protected Grid2D<T> grid;
        private Vector2 _debugGridPosition;

        protected void Awake()
        {
            _debugGridPosition = transform.position;
        }

        private void Update()
        {
            if(drawDebugGrid)
                MUtils.DrawDebugGrid(_debugGridPosition,1,rows,columns,Color.white);
        }

        public virtual void Refresh(Grid2D<T> grid)
        {
            
        }
    }
}