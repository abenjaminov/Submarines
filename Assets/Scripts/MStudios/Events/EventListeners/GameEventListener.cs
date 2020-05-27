using System;
using MStudios.Inspector;
using MStudios.Events.GameEvents;
using UnityEngine;

namespace MStudios.Events.EventListeners
{
    public abstract class GameEventListener<TEvent,TType> : MonoBehaviour where TEvent : GameEvent<TType>
    {
        public TEvent gameEvent;
        public ComponentActionSelector componentSelector;
        protected Action<TType> raiseAction;
        private Component _component;

        private void Awake()
        {
            InitializeAction();
        }
        
        void OnEnable()
        {
            Register();
        }

        private void OnDisable()
        {
            Unregister();
        }

        protected virtual void InitializeAction()
        {
            _component = GetComponent(componentSelector.componentName);
            var methodInfo = _component.GetType().GetMethod(componentSelector.methodName);

            if (methodInfo != null)
            {
                if (componentSelector.forceParameterType)
                {
                    raiseAction = value => methodInfo.Invoke(_component, new object[] {value});    
                }
                else
                {
                    raiseAction = value => methodInfo.Invoke(_component, new object[]{});
                }
            }
        }

        protected virtual void Register()
        {
            gameEvent.Subscribe(this.raiseAction);
        }

        protected virtual void Unregister()
        {
            gameEvent.Unsubscribe(this.raiseAction);
        }
    }
}