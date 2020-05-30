using System;
using System.Collections.Generic;
using MStudios;
using MStudios.Grid;
using Submarines.SideControllers;
using UnityEngine;

namespace Submarines
{
    public class SubsSide : GridVisual<SubmarineCellState>
    {
        public Submarine submarinePrefab;
        private readonly List<Submarine> _submarines = new List<Submarine>();
        private int _squaresAlive;

        private ISubSideController _sideController;

        protected override void Awake()
        {
            base.Awake();
            grid = new Grid2D<SubmarineCellState>(transform.position,rows, columns, this);
        }

        public void SetSideControllerAndActivate(ISubSideController sideController)
        {
            _sideController = sideController;
            _sideController.SetGrid(grid);
            _sideController.Activate();
        }
        
        public void DeactivateController()
        {
            _sideController?.Deactivate();
            _sideController = null;
        }

        public override void Refresh(Grid2D<SubmarineCellState> grid)
        {
            foreach (var drawnObject in _submarines)
            {
                Destroy(drawnObject.gameObject);
            }
            _submarines.Clear();
            
            var objectsOnGrid = grid.GetObjectsOnGrid();

            foreach (var objectOnGrid in objectsOnGrid)
            {
                var newSubObject = Instantiate(submarinePrefab,transform);
                newSubObject.transform.localPosition = objectOnGrid.gridReferenceFramePosition.AsVector3();
                
                var newSub = newSubObject.GetComponent<Submarine>();
                newSub.SetSprite(objectOnGrid.visual);
                
                _submarines.Add(newSub);
            }
        }
    }
}