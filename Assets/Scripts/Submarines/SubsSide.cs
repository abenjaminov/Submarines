using System;
using System.Collections.Generic;
using MStudios;
using MStudios.Grid;
using UnityEngine;

namespace Submarines
{
    [RequireComponent(typeof(ISubSideController))]
    public class SubsSide : GridVisual<SubmarineCellState>
    {
        
        private readonly List<SpriteRenderer> _objectsDrawnOnGrid = new List<SpriteRenderer>();
        private int _squaresAlive;

        private ISubSideController _sideController;

        private void Awake()
        {
            base.Awake();
            
            _sideController = GetComponent<ISubSideController>();
            grid = new Grid2D<SubmarineCellState>(transform.position,rows, columns, this);
        }

        public void PrepareForBattle()
        {
            _sideController.OnReadyForBattle += SideControllerOnOnReadyForBattle;
            _sideController.PrepareForBattle(grid, gridObjectsData);
        }

        private void SideControllerOnOnReadyForBattle()
        {
            Debug.Log("Side ready for battle");
        }

        public override void Refresh(Grid2D<SubmarineCellState> grid)
        {
            foreach (var drawnObject in _objectsDrawnOnGrid)
            {
                Destroy(drawnObject.gameObject);
            }
            _objectsDrawnOnGrid.Clear();
            
            var objectsOnGrid = grid.GetObjectsOnGrid();

            foreach (var objectOnGrid in objectsOnGrid)
            {
                var newRenderer =
                    MUtils.CreateSpriteObject2D(transform, objectOnGrid.gridReferenceFramePosition, objectOnGrid.visual, Color.white);
                _objectsDrawnOnGrid.Add(newRenderer);
            }
        }
    }
}