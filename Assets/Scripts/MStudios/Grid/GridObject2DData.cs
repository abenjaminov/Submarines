using System;
using System.Collections.Generic;
using UnityEngine;

namespace MStudios.Grid
{
    [CreateAssetMenu(menuName = "MStudios/Grid/Grid Object", fileName = "New Grid Object")]
    public class GridObject2DData : ScriptableObject
    {
        [HideInInspector] public int height;
        [HideInInspector] public int width;
        [HideInInspector] public Vector2 centerOffset;
        
        [HideInInspector] public float leftPartWidth;
        [HideInInspector] public float rightPartWidth;
        [HideInInspector] public float bottomPartHeight;
        [HideInInspector] public float topPartHeight;
        
        public Sprite visual;
        public List<Vector2Int> occupiedOffsets;

        private void OnEnable()
        {
            height = (int)(visual.rect.height / visual.pixelsPerUnit);
            width = (int)(visual.rect.width / visual.pixelsPerUnit);

            leftPartWidth = Mathf.Abs(visual.bounds.min.x);
            rightPartWidth = Mathf.Abs(visual.bounds.max.x);
            
            bottomPartHeight = Mathf.Abs(visual.bounds.min.y);
            topPartHeight = Mathf.Abs(visual.bounds.max.y);
            
            centerOffset = new Vector2(
                visual.pivot.x / visual.rect.width - 0.5f,
                visual.pivot.y / visual.rect.height - 0.5f
                );
        }
    }
}