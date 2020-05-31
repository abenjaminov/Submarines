using System.Collections;
using Submarines.SideControllers;
using UnityEngine;
using UnityEngine.iOS;

namespace Submarines.GameStates
{
    public class EnemyTurnState : GameState
    {
        public EnemyTurnState(BattleManager battleManager) : base(battleManager)
        {
            
        }

        public override IEnumerator Start()
        {
            yield return new WaitForSeconds(1);
            
            var aliveCellWorldPosition = battleManager.Player.GetRandomCellWorldPositionByState(SubmarineCellState.Alive);
            
            yield return new WaitForSeconds(1);
            
            battleManager.Player.DamageCell(aliveCellWorldPosition);
        }
    }
}