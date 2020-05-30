using UnityEngine;

namespace MStudios.Grid
{
    internal class GridObjectCell<T>
    {
        public Vector2Int localPosition;
        public T value;
    }
}