using System;
using System.Collections;
using System.Collections.Generic;
using MStudios.Grid;
using UnityEngine;

namespace Submarines
{
    public class AISubSideController : MonoBehaviour, ISubSideController
    {
        public event Action OnReadyForBattle;

        private List<GridObject2DData> _prepareForBattleObjects;
        
        public void PrepareForBattle(Grid2D<SubmarineCellState> grid, List<GridObject2DData> battleObjects)
        {
            _prepareForBattleObjects = battleObjects;
        }
    }
}