using MStudios;
using MStudios.Events.GameEvents;
using UnityEngine;

namespace Ships
{
    [CreateAssetMenu(menuName = "Unknown Seas/Events/Game Event/CannonBall Hit Target Game Event", fileName = "New CannonBallHitTargetGameEvent", order = 1)]
    public class CannonBallHitTargetGameEvent : GameEvent<CannonBallTargetHitInfo>
    {
        [ExecuteInEditMode]
        static CannonBallHitTargetGameEvent()
        {
            MUtils.Editor.AddType("Cannonball Target Hit Info", typeof(CannonBallTargetHitInfo));
        }
    }
}