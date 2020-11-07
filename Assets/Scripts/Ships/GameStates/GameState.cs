using System;
using System.Collections;

namespace Ships.GameStates
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

        protected void EndState(string message = "")
        {
            OnStateOver?.Invoke();
            
            if (message != string.Empty)
            {
                this.battleManager.Interface.ShowMessage(message);
            }

        }
    }
}