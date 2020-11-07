using System.Collections;
using Ships.SideControllers;
using UnityEngine;

namespace Ships.GameStates
{
    public class PlayerTurnState : GameState
    {
        private readonly PlayerTurnController _controller;
        public PlayerTurnState(BattleManager battleManager, PlayerTurnController turnController) : base(battleManager)
        {
            _controller = turnController;
        }

        public override IEnumerator Start()
        {
            this._controller.OnTurnEnd += ControllerOnOnTurnEnd;
            this._controller.OnGridLocationClicked += ControllerOnOnGridLocationClicked; 
            battleManager.Enemy.SetSideControllerAndActivate(this._controller);
            
            yield break;
        }

        private void ControllerOnOnGridLocationClicked(Vector2 position, ShipCellState obj)
        {
            battleManager.Player.FireAtTarget(position);

            this.EndPlayerTurn();
        }

        private void EndPlayerTurn()
        {
            this._controller.OnTurnEnd -= ControllerOnOnTurnEnd;
            this._controller.OnGridLocationClicked -= ControllerOnOnGridLocationClicked; 
            battleManager.Enemy.DeactivateController();
            this.EndState("Player turn end");
        }

        private void ControllerOnOnTurnEnd()
        {
            this.EndPlayerTurn();
        }
    }
}