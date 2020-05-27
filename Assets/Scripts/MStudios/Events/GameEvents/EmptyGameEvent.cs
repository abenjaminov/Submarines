using System;
using UnityEngine;

namespace MStudios.Events.GameEvents
{
    [CreateAssetMenu(menuName = "MStudios/Events/Game Event/Empty Event", fileName = "New Simple Event", order = 1)]
    public class EmptyGameEvent : GameEvent<object>
    {
        protected new event Action OnRaised;
        
        public void Subscribe(Action action)
        {
            OnRaised += action;
        }
        
        public void Unsubscribe(Action action)
        {
            OnRaised -= action;
        }

        public void Raise()
        {
            OnRaised?.Invoke();
        }
    }
}