namespace Submarines
{
    public class GameManager
    {
        private GameState currentState;
        
        private void Awake()
        {
            currentState = new PrepareForBattleState();    
        }
    }
}