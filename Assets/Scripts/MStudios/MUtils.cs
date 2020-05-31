using System;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace MStudios
{
    public static class MUtils
    {
        public static class Mouse
        {
            public static Vector3 GetWorldPosition(Camera camera)
            {
                var worldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
                return worldPoint;
            }    
        }

        public static TextMeshPro CreateTextMesh(Transform parent, Vector3 position, string text)
        {
            var newText = new GameObject(text, typeof(TextMeshPro));
            newText.transform.SetParent(parent, false);
            newText.transform.localPosition = position;
            var textMesh = newText.GetComponent<TextMeshPro>();
            textMesh.text = text;
            textMesh.color = Color.black;
            textMesh.fontSize = 2;
            textMesh.alignment = TextAlignmentOptions.Center;

            return textMesh;
        }
        
        public static void DrawDebugGrid(Vector2 startPosition, float cellSize, int rows, int columns, Color color, bool showIndexes = false)
        {
            var offsetStartPosition = startPosition + new Vector2(-0.5f, -0.5f);
            var rightMax = offsetStartPosition.x + (cellSize * columns);
            var topMax = offsetStartPosition.y + (cellSize * rows);
            
            Vector2 gridRightBottomEdge = new Vector2(rightMax, offsetStartPosition.y);
            Vector2 gridRightTopEdge = new Vector2(rightMax, topMax);
            Vector2 gridLeftTopEdge = new Vector2(offsetStartPosition.x, topMax);
            
            Debug.DrawLine(offsetStartPosition, gridRightBottomEdge, color);
            Debug.DrawLine(gridRightTopEdge, gridRightBottomEdge, color);
            Debug.DrawLine(offsetStartPosition, gridLeftTopEdge, color);
            Debug.DrawLine(gridRightTopEdge, gridLeftTopEdge, color);

            for (int i = 0; i < columns; i++)
            {
                var currentStart = new Vector2(offsetStartPosition.x + i * cellSize, topMax);
                var currentEnd = new Vector2(offsetStartPosition.x + i * cellSize, offsetStartPosition.y); 
                
                Debug.DrawLine(currentEnd, currentStart,color);
            }
            
            for (int i = 0; i < rows; i++)
            {
                var currentStart = new Vector2(rightMax, offsetStartPosition.y + i * cellSize);
                var currentEnd = new Vector2(offsetStartPosition.x,offsetStartPosition.y + i * cellSize); 
                
                Debug.DrawLine(currentEnd, currentStart,color);
            }

        }
        
        public static SpriteRenderer CreateSpriteObject2D(Transform parent, Vector2 localPosition, Sprite sprite, Color color,int layerSortingOrder = 0)
        {
            GameObject gameObject = new GameObject("Sprite2D", typeof(SpriteRenderer));
            Transform transform = gameObject.transform;
            transform.SetParent(parent);
            transform.localPosition = localPosition;

            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = layerSortingOrder;
            renderer.color = color;

            return renderer;
        }
    }
}