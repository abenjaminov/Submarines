using System;
using MStudios.Grid;
using UnityEngine;

namespace Submarines
{
    public abstract class GameJudge : MonoBehaviour
    {
        private Grid2D<SubmarineCellState> playerGrid;
        private Grid2D<SubmarineCellState> enemyGrid;
    }
}