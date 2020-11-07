using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MStudios;
using MStudios.Grid;
using Ships.SideControllers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ships
{
    public class ShipSide : GridVisual<ShipCellState>
    {
        public event Action OnSideLost;
        
        
        [SerializeField] private Ship shipPrefab;
        private readonly List<Ship> _ships = new List<Ship>();
        [SerializeField] private List<Cannon> landCannons;
        [SerializeField] private GameObject burningEffectPrefab;
        [SerializeField] private List<GameObject> instantiatedBurningEffects = new List<GameObject>();
        [SerializeField] private bool isFriendly;
        [SerializeField] private ExpandingCircle expandingCircleEffect;
        private IShipSideController _sideController;
        private bool _expectImpact;

        protected override void Awake()
        {
            base.Awake();
            grid = new Grid2D<ShipCellState>(transform.position,rows, columns, this);

            if (isFriendly)
            {
                foreach (var cannon in landCannons)
                {
                    cannon.SetAsFriendly();
                }    
            }
        }

        public void SetSideControllerAndActivate(IShipSideController sideController)
        {
            _sideController = sideController;
            _sideController.SetGrid(grid);
            _sideController.Activate();
        }

        public void ClearSideController()
        {
            _sideController.Deactivate();
            _sideController = null;
        }
        
        public void FireAtTarget(Vector2 worldPosition)
        {
            var localGridPosition = grid.SnapToLocalGridPosition(worldPosition);
            
            var height = new Vector2Int(0, localGridPosition.y);

            var objectsOnGridAtSameHeight = grid.GetObjectsOnGrid()
                .Where(x => x.IsPositionOnObject(height.With(x: x.gridReferenceFramePosition.x))).ToList();

            if (objectsOnGridAtSameHeight.Count > 0)
            {
                var randomObject = objectsOnGridAtSameHeight[Random.Range(0, objectsOnGridAtSameHeight.Count)];
                var shipAttacker = _ships.FirstOrDefault(ship =>
                    ship.transform.localPosition == randomObject.gridReferenceFramePosition.AsVector3());

                if (shipAttacker != null)
                {
                    shipAttacker.FireAt(worldPosition);
                }
            }
            else
            {
                var randomCannon = landCannons[Random.Range(0, landCannons.Count)];
                
                randomCannon.FireAt(worldPosition);
            }
        }
        
        public void DeactivateController()
        {
            _sideController?.Deactivate();
            _sideController = null;
        }

        public override void Refresh()
        {
            foreach (var drawnObject in _ships)
            {
                Destroy(drawnObject.gameObject);
            }

            _ships.Clear();
            
            var objectsOnGrid = grid.GetObjectsOnGrid();

            foreach (var objectOnGrid in objectsOnGrid)
            {
                var newShip = GetNewShip(objectOnGrid);

                ConfigureShip(objectOnGrid, newShip);

                _ships.Add(newShip);
            }
        }

        private void ConfigureShip(GridObject2D<ShipCellState> objectOnGrid, Ship newShip)
        {
            var deadPositions = objectOnGrid.GetAllLocalPositionsWithValue(ShipCellState.Dead);
            foreach (var deadPosition in deadPositions)
            {
                newShip.SetDeadCell(deadPosition);
            }
        }

        private Ship GetNewShip(GridObject2D<ShipCellState> objectOnGrid)
        {
            var newSubObject = Instantiate(shipPrefab, transform);
            newSubObject.transform.localPosition = objectOnGrid.gridReferenceFramePosition.AsVector3();

            var newShip = newSubObject.GetComponent<Ship>();
            newShip.SetSprite(objectOnGrid.visual);
            
            if (isFriendly)
            {
                newShip.SetAsFriendly();
            }
            
            return newShip;
        }

        private void OnShipHit(Vector3 worldPosition, CannonBall hitBall)
        {
            if (grid.GetValueAt(worldPosition) == ShipCellState.Alive)
            {
                grid.SetValue(ShipCellState.Dead, worldPosition);
                var snappedWorldPosition = grid.SnapToWorldGridPosition(worldPosition);
                var burningEffect = Instantiate(burningEffectPrefab, transform);
                burningEffect.transform.position = snappedWorldPosition;
                Destroy(hitBall.gameObject);
                
                var gridHasLiveCell = grid.HasCellWithValue(ShipCellState.Alive);

                if (!gridHasLiveCell)
                {
                    OnSideLost?.Invoke();
                }
            }
        }

        public Vector3 GetRandomCellWorldPositionByState(ShipCellState state)
        {
            return grid.GetRandomLocalCellPositionByValue(state).AsVector3() + transform.position;
        }

        public bool HasLiveCells()
        {
            var gridHasLiveCell = grid.HasCellWithValue(ShipCellState.Alive);

            return gridHasLiveCell;
        }

        public void OnCannonBallHitTarget(CannonBallTargetHitInfo cannonBallTargetHitInfo)
        {
            if (this.isFriendly == cannonBallTargetHitInfo.cannonBall.IsFriendly())
                return;
            
            var valueAtHitTarget = this.grid.GetValueAt(cannonBallTargetHitInfo.target);

            if (valueAtHitTarget == ShipCellState.Alive)
            {
                if (cannonBallTargetHitInfo.cannonBall.GetTimeAlive() > 0.1f)
                {
                    this.OnShipHit(cannonBallTargetHitInfo.target, cannonBallTargetHitInfo.cannonBall);
                }
            }
            else if (valueAtHitTarget == ShipCellState.Empty)
            {
                StartCoroutine(InstantiateExpandingCircle(0, cannonBallTargetHitInfo.target));
                StartCoroutine(InstantiateExpandingCircle(0.2f, cannonBallTargetHitInfo.target));
                StartCoroutine(InstantiateExpandingCircle(0.4f, cannonBallTargetHitInfo.target));
            }
            
            Destroy(cannonBallTargetHitInfo.cannonBall.gameObject);
        }

        public IEnumerator InstantiateExpandingCircle(float delay, Vector3 position)
        {
            yield return new WaitForSeconds(delay);
            Instantiate(expandingCircleEffect, position, Quaternion.identity);
        }
    }
}