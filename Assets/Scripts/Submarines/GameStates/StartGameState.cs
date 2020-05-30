using System.Collections;
using UnityEngine;

namespace Submarines.GameStates
{
    public class StartGameState : GameState
    {
        private int _countdownToStart;
        
        public StartGameState(BattleManager battleManager, int countdownToStart) : base(battleManager)
        {
            _countdownToStart = countdownToStart;
        }

        public override IEnumerator Start()
        {
            for (int i = _countdownToStart; i > 0; i--)
            {
                yield return new WaitForSeconds(1);
                Debug.Log(i);
            }
            
            this.EndState();
        }
    }
}