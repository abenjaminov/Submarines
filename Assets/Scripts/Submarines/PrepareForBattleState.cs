using System.Collections.Generic;
using MStudios.Grid;
using UnityEngine;

namespace Submarines
{
    public class PrepareForBattleState : GameState
    {
        private List<GridObject2DData> _prepareForBattleObjects;
        private int _selectedObjectIndex = 0;
        
        public PrepareForBattleState(List<GridObject2DData> battleObjects)
        {
            _prepareForBattleObjects = battleObjects;
        }

        
    }
}