using System.Collections.Generic;
using UnityEngine;

namespace MStudios.Events.GameEvents
{
    
    [CreateAssetMenu(menuName = "MStudios/Events/Game Event/Position 3D Event", fileName = "New Position Event", order = 2)]
    public class Position3DGameEvent : GameEvent<Vector3>
    {
        public List<GameObject> listeners;
    }
}