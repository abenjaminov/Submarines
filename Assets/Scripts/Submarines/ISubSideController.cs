using System;
using System.Collections;
using System.Collections.Generic;
using MStudios.Grid;

namespace Submarines
{
    public interface ISubSideController
    {
        event Action OnReadyForBattle;
        void PrepareForBattle(Grid2D<SubmarineCellState> grid, List<GridObject2DData> battleObjects);
    }
}