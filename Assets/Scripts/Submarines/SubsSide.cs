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
        [SerializeField] private Submarine submarinePrefab;
        private readonly List<Submarine> _submarines = new List<Submarine>();

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

        public override void Refresh()
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

                var deadPositions = objectOnGrid.GetAllLocalPositionsWithValue(SubmarineCellState.Dead);
                foreach (var deadPosition in deadPositions)
                {
                    newSub.SetDeadCell(deadPosition);
                }

                _submarines.Add(newSub);
            }
        }

        public Vector3 GetRandomCellWorldPositionByState(SubmarineCellState state)
        {
            return grid.GetRandomCellByValue(state).AsVector3() + transform.position;
        }

        public void DamageCell(Vector3 cellWorldPosition)
        {
            grid.SetValue(SubmarineCellState.Dead, cellWorldPosition);
        }        
    }
}