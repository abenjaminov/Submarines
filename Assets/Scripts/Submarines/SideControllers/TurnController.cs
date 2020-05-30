using System;
using System.Collections;
using MStudios;
using MStudios.Grid;
using UnityEngine;

namespace Submarines.SideControllers
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TurnController : MonoBehaviour, ISubSideController
    {
        public event Action OnTurnEnd;
        public event Action OnGridLocationClicked;
        
        private Grid2D<SubmarineCellState> _grid;
        [SerializeField] private GridObject2DData cellSelectorObject;
        private SpriteRenderer _cellSelectorRenderer;
        private Camera _mainCamera;
        private BoxCollider2D _boxCollider2D;
        
        public float turnLength;
        private float _activeTurnLength = 0;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        public void SetGrid(Grid2D<SubmarineCellState> grid)
        {
            _grid = grid;
            transform.position = grid.gridPosition;
            SetGridCollider();
        }

        private void SetGridCollider()
        {
            _boxCollider2D.size = new Vector2(_grid.columns, _grid.rows);
            
            var halfCellSize = 0.5f;
            _boxCollider2D.offset = new Vector2((_grid.columns / 2f) - halfCellSize, (_grid.rows / 2f) - halfCellSize);
        }
        
        public void Activate()
        {
            _activeTurnLength = 0;
            var localPosition = GetMouseLocalGridPosition();
            _cellSelectorRenderer =
                MUtils.CreateSpriteObject2D(transform, localPosition, cellSelectorObject.visual, Color.white);
        }

        private Vector2 GetMouseLocalGridPosition()
        {
            var mouseWorldPosition = MUtils.Mouse.GetWorldPosition(_mainCamera);
            var localPosition = _grid.SnapToWorldGridPosition(mouseWorldPosition, cellSelectorObject);
            return localPosition;
        }

        public void Deactivate()
        {
            Destroy(_cellSelectorRenderer.gameObject);
            _cellSelectorRenderer = null;
        }

        private void Update()
        {
            if (_cellSelectorRenderer == null) return;

            _cellSelectorRenderer.transform.position = GetMouseLocalGridPosition();

            _activeTurnLength += Time.deltaTime;

            if (_activeTurnLength >= turnLength)
            {
                this.EndTurn();
            }
        }

        private void EndTurn()
        {
            OnTurnEnd?.Invoke();
        }

        private void OnMouseDown()
        {
            var mouseWorldPosition = MUtils.Mouse.GetWorldPosition(_mainCamera);

            SubmarineCellState state = _grid.GetValueAt(mouseWorldPosition);

            if (state == SubmarineCellState.Alive)
            {
                _grid.SetValue(SubmarineCellState.Dead, mouseWorldPosition);
                Debug.Log("Hit");
            }
        }
    }
}