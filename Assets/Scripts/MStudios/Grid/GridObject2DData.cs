using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GridUtils
{
    [CreateAssetMenu(menuName = "Grid/Grid Object", fileName = "New Grid Object")]
    public class GridObject2DData : ScriptableObject
    {
        public int height;
        public int width;
        public Sprite visual;
        public List<Vector2Int> occupiedOffsets;
        
        public Vector2 pivot => visual.bounds.center;
        
    }
}