using System.Collections;
using System.Collections.Generic;
using MStudios.Grid;
using Submarines.SideControllers;
using UnityEngine;

namespace Submarines.GameStates
{
    public class PrepareForBattleState : GameState
    {
        private readonly IPrepareForBattleSubSideController _playerController;
        private readonly IPrepareForBattleSubSideController _enemyController;

        private bool _playerReady;
        private bool _enemyReady;
        
        public PrepareForBattleState(BattleManager battleManager,
            PlayerPrepareForBattleSideController playerController,
            AIPrepareForBattleSideController enemyController,  
            List<GridObject2DData> battleObjects) : base(battleManager)
        {
            this._playerController = playerController;
            this._enemyController = enemyController;
        }

        public override IEnumerator Start()
        {
            this._playerController.OnReadyForBattle += PlayerControllerOnOnReadyForBattle;
            this._enemyController.OnReadyForBattle += EnemyControllerOnOnReadyForBattle;
            
            battleManager.Player.SetSideControllerAndActivate(this._playerController);
            battleManager.Enemy.SetSideControllerAndActivate(this._enemyController);

            yield break;
        }

        private void PlayerControllerOnOnReadyForBattle()
        {
            _playerReady = true;
            battleManager.Player.DeactivateController();
            TryEndState();
        }

        private void EnemyControllerOnOnReadyForBattle()
        {
            _enemyReady = true;
            battleManager.Enemy.DeactivateController();
            TryEndState();
        }

        private void TryEndState()
        {
            if (!_enemyReady || !_playerReady) return;

            this.EndState();
        }
    }
}