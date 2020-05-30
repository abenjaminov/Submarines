using System.Collections;
using Submarines.SideControllers;

namespace Submarines.GameStates
{
    public class PlayerTurnState : GameState
    {
        private readonly TurnController _controller;
        public PlayerTurnState(BattleManager battleManager, TurnController turnController) : base(battleManager)
        {
            _controller = turnController;
        }

        public override IEnumerator Start()
        {
            this._controller.OnTurnEnd += ControllerOnOnTurnEnd;
            battleManager.Enemy.SetSideControllerAndActivate(this._controller);

            yield break;
        }

        private void ControllerOnOnTurnEnd()
        {
            this.EndState();
        }
    }
}