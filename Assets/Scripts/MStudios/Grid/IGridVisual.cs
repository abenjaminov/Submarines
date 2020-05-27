using UnityEngine;

namespace MStudios.Grid
{
    public interface IGridVisual<T>
    {
        void Refresh(Grid2D<T> grid);
    }
}