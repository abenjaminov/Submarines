using System.Data.SqlTypes;
using MStudios.Grid;

namespace Submarines.SideControllers
{
    public interface ISubSideController
    {
        void SetGrid(Grid2D<SubmarineCellState> grid);
        void Activate();
        void Deactivate();
    }
}