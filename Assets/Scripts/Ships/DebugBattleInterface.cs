using UnityEngine;

namespace Ships
{
    public class DebugBattleInterface : MonoBehaviour, IBattleManagerInterface
    {
        public void ShowMessage(string message)
        {
            Debug.Log("Battle Interface - " + message);
        }
    }
}