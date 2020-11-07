using System.Data.SqlTypes;
using MStudios.Grid;

namespace Ships.SideControllers
{
    public interface IShipSideController
    {
        void SetGrid(Grid2D<ShipCellState> grid);
        void Activate();
        void Deactivate();
    }
}