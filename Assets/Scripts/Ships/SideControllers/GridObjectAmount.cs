using System;
using MStudios.Grid;

namespace Submarines.SideControllers
{
    [Serializable]
    public class GridObjectAmount
    {
        public GridObject2DData objectData;
        public int amount;
        public bool empty => amount == 0;
    }
}