using System.Collections.Generic;
using MStudios.Events.GameEvents;
using MStudios.Grid;
using UnityEngine;

namespace Submarines
{
    public class BattleManager : MonoBehaviour
    {
        public List<GridObject2DData> submarines;
        private GameState _currentState;

        [Space]
        [Header("State Events")]
        public EmptyGameEvent prepareForBattleStateStarted;
        
        private void Awake()
        {
              StartPrepareForBattle();
        }

        private void StartPrepareForBattle()
        {
            _currentState = new PrepareForBattleState(submarines);
            prepareForBattleStateStarted.Raise();
        }
    }
}