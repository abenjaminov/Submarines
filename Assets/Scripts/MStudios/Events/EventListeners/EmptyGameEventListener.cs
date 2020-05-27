using System;
using MStudios.Events.GameEvents;

namespace MStudios.Events.EventListeners
{
    public class EmptyGameEventListener : GameEventListener<EmptyGameEvent, object>
    {
        private Action _emptyRaiseAction;
        
        protected override void InitializeAction()
        {
            base.InitializeAction();
            
            _emptyRaiseAction = new Action(() =>
            {
                this?.raiseAction(null);
            });
            componentSelector.forceParameterType = false;
        }

        protected override void Register()
        {
            gameEvent.Subscribe(_emptyRaiseAction);
        }

        protected override void Unregister()
        {
            gameEvent.Unsubscribe(_emptyRaiseAction);
        }
    }
}