using System.Collections;
using Ships.SideControllers;
using UnityEngine;

namespace Ships.GameStates
{
    public class EnemyTurnState : GameState
    {
        private AITurnController _controller;
        
        public EnemyTurnState(BattleManager battleManager, AITurnController controller) : base(battleManager)
        {
            _controller = controller;
        }

        public override IEnumerator Start()
        {
            _controller.OnTurnEnd += ControllerOnOnTurnEnd;
            battleManager.Player.SetSideControllerAndActivate(_controller);

            yield break;
        }

        private void ControllerOnOnTurnEnd()
        {
            _controller.OnTurnEnd -= ControllerOnOnTurnEnd;
            battleManager.Player.DeactivateController();
            
            this.EndState("Enemy turn end");
        }
    }
}