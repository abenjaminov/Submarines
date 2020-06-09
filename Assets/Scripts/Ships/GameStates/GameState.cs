using System;
using System.Collections;

namespace Submarines.GameStates
{
    public abstract class GameState
    {
        public event Action OnStateOver;
        protected BattleManager battleManager;

        protected GameState(BattleManager battleManager)
        {
            this.battleManager = battleManager;
        }

        public abstract IEnumerator Start();

        protected void EndState()
        {
            OnStateOver?.Invoke();
        }
    }
}