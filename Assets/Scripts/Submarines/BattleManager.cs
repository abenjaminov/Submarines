using System.Collections.Generic;
using MStudios.Events.GameEvents;
using MStudios.Grid;
using Submarines.GameStates;
using Submarines.SideControllers;
using UnityEngine;

namespace Submarines
{
    public class BattleManager : MonoBehaviour
    {
        public List<GridObject2DData> submarines;
        private GameState _currentState;

        [Space] [Header("Side Controllers")]
        [SerializeField] private PlayerPrepareForBattleSideController playerPrepareForBattleSideController;
        [SerializeField] private AIPrepareForBattleSideController aiPrepareForBattleSideController;
        [SerializeField] private TurnController playerTurnController;

        [Space] [Header("Sides (Grids)")]
        public SubsSide Player;
        public SubsSide Enemy;
        
        private void Awake()
        {
              StartPrepareForBattle();
        }

        private void StartPrepareForBattle()
        {
            _currentState = new PrepareForBattleState(this,playerPrepareForBattleSideController, aiPrepareForBattleSideController, submarines);
            _currentState.OnStateOver += PrepareForBattleStateOver;
            _currentState.Start();
            StartCoroutine(_currentState.Start());
        }

        private void PrepareForBattleStateOver()
        {
            _currentState = new StartGameState(this, 5);
            _currentState.OnStateOver += StartGameStateOver;
            StartCoroutine(_currentState.Start());
        }

        private void StartGameStateOver()
        {
            _currentState = new PlayerTurnState(this, playerTurnController);
            StartCoroutine(_currentState.Start());
        }
    }
}