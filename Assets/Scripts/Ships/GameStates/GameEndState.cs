using System.Collections;

namespace Ships.GameStates
{
    public class GameEndState : GameState
    {
        private bool _playerWon;
        public GameEndState(BattleManager battleManager, bool playerWon) : base(battleManager)
        {
            _playerWon = playerWon;
        }

        public override IEnumerator Start()
        {
            battleManager.Player.DeactivateController();
            battleManager.Enemy.DeactivateController();
            
            if (_playerWon)
            {
                battleManager.Interface.ShowMessage("Player Won");
            }
            else
            {
                battleManager.Interface.ShowMessage("Player Lost");
            }

            yield break;
        }
    }
}