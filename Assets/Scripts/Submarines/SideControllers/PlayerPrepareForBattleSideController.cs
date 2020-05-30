using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MStudios;
using MStudios.Grid;
using Submarines.SideControllers;
using UnityEngine;

namespace Submarines
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerPrepareForBattleSideController : MonoBehaviour,  IPrepareForBattleSubSideController
    {
        public event Action OnReadyForBattle;
        
        private Grid2D<SubmarineCellState> _grid;
        public List<GridObjectAmount> prepareForBattleObjects;       
        private int _selectedObjectIndex = -1;
        private SpriteRenderer _selectedObjectRenderer;
        private BoxCollider2D _boxCollider2D;

        private GridObject2DData gridObjectData => prepareForBattleObjects[_selectedObjectIndex].objectData;
        
        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        public void SetGrid(Grid2D<SubmarineCellState> grid)
        {
            _grid = grid;
            transform.position = grid.gridPosition;
            SetGridCollider();
        }

        public void Activate()
        {
            ToggleSelectedGridObject();
        }
        
        public void Deactivate()
        {
            _grid = null;
        }

        private void SetGridCollider()
        {
            _boxCollider2D.size = new Vector2(_grid.columns, _grid.rows);
            
            var halfCellSize = 0.5f;
            _boxCollider2D.offset = new Vector2((_grid.columns / 2f) - halfCellSize, (_grid.rows / 2f) - halfCellSize);
        }
        
        private void Update()
        {
            if (_grid == null || IsReadyForBattle()) return;
            
            if (Input.GetKeyDown(KeyCode.T))
            {
                ToggleSelectedGridObject();
            }

            UpdateSelectedGridObject();
        }

        private void UpdateSelectedGridObject()
        {
            var mouseWorldPosition = MUtils.Mouse.GetWorldPosition(Camera.main);
            var positionOnGrid =
                _grid.SnapToGridInWorld(mouseWorldPosition, gridObjectData);
            _selectedObjectRenderer.gameObject.transform.position = positionOnGrid;
            _selectedObjectRenderer.color = Color.white;
            
            if(!_grid.CanPutDownObject(gridObjectData, mouseWorldPosition))
            {
                _selectedObjectRenderer.color = Color.red;   
            }
        }

        private void ToggleSelectedGridObject()
        {
            _selectedObjectIndex = (_selectedObjectIndex + 1) % prepareForBattleObjects.Count;

            while (prepareForBattleObjects[_selectedObjectIndex].empty)
            {
                _selectedObjectIndex = (_selectedObjectIndex + 1) % prepareForBattleObjects.Count;
            }
            
            if (_selectedObjectRenderer != null)
            {
                Destroy(_selectedObjectRenderer.gameObject);
            }

            _selectedObjectRenderer = MUtils.CreateSpriteObject2D(transform, Vector2.zero,
                gridObjectData.visual, Color.white);
            _selectedObjectRenderer.sortingOrder = 1;
        }

        private void OnMouseDown()
        {
            var selectedObject = prepareForBattleObjects[_selectedObjectIndex];
            var mouseWorldPosition = MUtils.Mouse.GetWorldPosition(Camera.main);

            if (_grid.CanPutDownObject(selectedObject.objectData, mouseWorldPosition))
            {
                _grid.PutDownObject(selectedObject.objectData, mouseWorldPosition, SubmarineCellState.Alive);
                
                selectedObject.amount--;

                if (IsReadyForBattle())
                {
                    Destroy(_selectedObjectRenderer);
                    OnReadyForBattle?.Invoke();
                }
                else if(selectedObject.amount == 0)
                {
                    ToggleSelectedGridObject();
                }
            }
        }

        private bool IsReadyForBattle()
        {
            return prepareForBattleObjects.All(x => x.amount == 0);
        }
    }
}