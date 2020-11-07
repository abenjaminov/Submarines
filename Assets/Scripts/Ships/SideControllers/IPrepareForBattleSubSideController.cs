using System;

namespace Ships.SideControllers
{
    public interface IPrepareForBattleSubSideController : IShipSideController
    {
        event Action OnReadyForBattle;
    }
}