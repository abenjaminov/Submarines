using System;
using System.Collections;
using System.Collections.Generic;
using MStudios;
using MStudios.Grid;
using UnityEngine;

namespace Submarines
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerSubSideController : MonoBehaviour,  ISubSideController
    {
        public event Action OnReadyForBattle;

        private Grid2D<SubmarineCellState> _grid;
        private List<GridObject2DData> _prepareForBattleObjects;       
        private int _selectedObjectIndex = -1;
        private SpriteRenderer _selectedObjectRenderer;

        private BoxCollider2D _boxCollider2D;

        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }
        
        public void PrepareForBattle(Grid2D<SubmarineCellState> grid, List<GridObject2DData> battleObjects)
        {
            _grid = grid;
            _prepareForBattleObjects = battleObjects;
            ToggleSelectedGridObject();
            SetGridCollider();
        }

        private void SetGridCollider()
        {
            _boxCollider2D.size = new Vector2(_grid.columns, _grid.rows);
            
            var halfCellSize = 0.5f;
            _boxCollider2D.offset = new Vector2((_grid.columns / 2f) - halfCellSize, (_grid.rows / 2f) - halfCellSize);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                ToggleSelectedGridObject();
            }

            var mouseWorldPosition = MUtils.Mouse.GetWorldPosition(Camera.main);
            var positionOnGrid =
                _grid.SnapToGridInWorld(mouseWorldPosition, _prepareForBattleObjects[_selectedObjectIndex]);
            _selectedObjectRenderer.gameObject.transform.position = positionOnGrid;
        }

        private void ToggleSelectedGridObject()
        {
            _selectedObjectIndex = (_selectedObjectIndex + 1) % _prepareForBattleObjects.Count;

            if (_selectedObjectRenderer != null)
            {
                Destroy(_selectedObjectRenderer.gameObject);
            }

            _selectedObjectRenderer = MUtils.CreateSpriteObject2D(transform, Vector2.zero,
                _prepareForBattleObjects[_selectedObjectIndex].visual, Color.white);
        }

        private void OnMouseDown()
        {
            var selectedObject = _prepareForBattleObjects[_selectedObjectIndex];
            var mouseWorldPosition = MUtils.Mouse.GetWorldPosition(Camera.main);

            if (_grid.CanPutDownObject(selectedObject, mouseWorldPosition))
            {
                _grid.PutDownObject(selectedObject, mouseWorldPosition, SubmarineCellState.Alive);
            }
        }
    }
}