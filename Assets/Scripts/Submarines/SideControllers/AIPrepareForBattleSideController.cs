using System;
using System.Collections.Generic;
using MStudios.Grid;
using UnityEngine;

namespace Submarines.SideControllers
{
    public class AIPrepareForBattleSideController : MonoBehaviour, IPrepareForBattleSubSideController
    {
        private Grid2D<SubmarineCellState> _grid;
        public List<GridObject2DData> prepareForBattleObjects;
        public event Action OnReadyForBattle;
        public void SetGrid(Grid2D<SubmarineCellState> grid)
        {
            _grid = grid;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            _grid.PutDownObject(prepareForBattleObjects[0], transform.position, SubmarineCellState.Alive);
            OnReadyForBattle?.Invoke();
        }
        
        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}