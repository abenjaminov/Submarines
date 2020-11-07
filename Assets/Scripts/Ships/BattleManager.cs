using Ships.GameStates;
using Ships.SideControllers;
using UnityEngine;

namespace Ships
{
    public class BattleManager : MonoBehaviour
    {
        private GameState _currentState;

        [Space] [Header("Side Controllers")]
        [SerializeField] private PlayerPrepareForBattleSideController playerPrepareForBattleSideController;
        [SerializeField] private AIPrepareForBattleSideController aiPrepareForBattleSideController;
        [SerializeField] private PlayerTurnController playerTurnController;
        [SerializeField] private AITurnController AITurnController;

        [Space] [Header("Sides (Grids)")]
        public ShipSide Player;
        public ShipSide Enemy;

        [HideInInspector] public IBattleManagerInterface Interface;
        
        private bool _isPlayerTurn;
        
        private void Awake()
        {
            Interface = GetComponent<IBattleManagerInterface>();

            Player.OnSideLost += PlayerSideLost;
            Enemy.OnSideLost += EnemySideLost;
            
            StartPrepareForBattle();
        }

        private void StartPrepareForBattle()
        {
            _currentState = new PrepareForBattleState(this,playerPrepareForBattleSideController, aiPrepareForBattleSideController);
            _currentState.OnStateOver += PrepareForBattleStateOver;
            _currentState.Start();
            StartCoroutine(_currentState.Start());
        }

        private void PrepareForBattleStateOver()
        {
            ToggleTurns();
        }

        private void ToggleTurns()
        {
            _isPlayerTurn = !_isPlayerTurn;

            if (_isPlayerTurn)
            {
                _currentState = new PlayerTurnState(this, playerTurnController);
                StartCoroutine(_currentState.Start());
            }
            else
            {
                _currentState = new EnemyTurnState(this,AITurnController);
                StartCoroutine(_currentState.Start());
            }
            
            _currentState.OnStateOver += ToggleTurns;
        }

        private void PlayerSideLost()
        {
            _currentState = new GameEndState(this, false);
            StartCoroutine(_currentState.Start());
        }
        
        private void EnemySideLost()
        {
            _currentState = new GameEndState(this, true);
            StartCoroutine(_currentState.Start());
        }
    }
}