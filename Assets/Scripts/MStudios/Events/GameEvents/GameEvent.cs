using System;
using System.Collections.Generic;
using UnityEngine;

namespace MStudios.Events.GameEvents
{
    public class GameEvent<T> : ScriptableObject
    {
        protected event Action<T> OnRaised;
        
        public void Subscribe(Action<T> action)
        {
            OnRaised += action;
        }
        
        public void Unsubscribe(Action<T> action)
        {
            OnRaised -= action;
        }

        public void Raise(T data)
        {
            OnRaised?.Invoke(data);
        }
    }
}