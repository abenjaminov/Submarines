using System.Collections.Generic;
using UnityEngine;

namespace MStudios.Events.GameEvents
{
    
    [CreateAssetMenu(menuName = "MStudios/Events/Game Event/Position 2D Event", fileName = "New Position Event", order = 4)]
    public class Position2DGameEvent : GameEvent<Vector2>
    {
        public List<GameObject> listeners;
    }
}