using System;

namespace Submarines.SideControllers
{
    public interface IPrepareForBattleSubSideController : ISubSideController
    {
        event Action OnReadyForBattle;
    }
}