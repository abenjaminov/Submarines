
using UnityEngine;

namespace MStudios.Grid
{
    public interface IGrid2D
    {
        bool IsCellEmpty(Vector2 worldPosition);
        bool CanPutDownObject(GridObject2DData objectData, Vector2 worldPosition);
    }
}