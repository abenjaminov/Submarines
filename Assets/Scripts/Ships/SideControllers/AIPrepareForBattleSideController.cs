using System;
using System.Collections.Generic;
using MStudios.Grid;
using UnityEngine;

namespace Ships.SideControllers
{
    public class AIPrepareForBattleSideController : MonoBehaviour, IPrepareForBattleSubSideController
    {
        private Grid2D<ShipCellState> _grid;
        public List<GridObject2DData> prepareForBattleObjects;
        public event Action OnReadyForBattle;
        public void SetGrid(Grid2D<ShipCellState> grid)
        {
            _grid = grid;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            OnReadyForBattle?.Invoke();
        }
        
        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}