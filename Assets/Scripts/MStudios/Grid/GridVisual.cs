using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

namespace GridUtils
{
    public class GridVisual : MonoBehaviour
    {
        [SerializeField] private bool drawDebugGrid;
        [SerializeField] private List<GridObject2DData> gridObjectsData;
        [SerializeField] private int rows;
        [SerializeField] private int columns;
        
        private Grid2D<int> _grid;
        private Vector2 gridPosition;
        private Sprite selectionBoxSprite;
        private SpriteRenderer mouseFollowRenderer;
        private BoxCollider2D boxCollider2D;
        private GridObject2DData selectedGridObject;

        private void Awake()
        {
            gridPosition = transform.position - new Vector3(columns / 2, (rows / 2),0);
            _grid = new Grid2D<int>(transform.position,rows, columns);
            boxCollider2D = GetComponent<BoxCollider2D>();
            boxCollider2D.size = new Vector2(columns, rows);
            selectedGridObject = gridObjectsData[0];
        }

        public void StartFollowMouse()
        {
            if (mouseFollowRenderer == null)
            {
                mouseFollowRenderer =
                    ABenjUtils.CreateSpriteObject2D(transform, Input.mousePosition, selectedGridObject.visual, Color.white);
            }
            
            mouseFollowRenderer.gameObject.SetActive(true);
        }
        
        private void Start()
        {
            StartFollowMouse();
        }

        private void Update()
        {
            if(drawDebugGrid)
                ABenjUtils.DrawDebugGrid(gridPosition,1,rows,columns,Color.white);
            
            var mouseInWorld = ABenjUtils.Mouse.GetWorldPosition(Camera.main);
            var positionOnGrid = _grid.SnapToGridInWorld(mouseInWorld,selectedGridObject);
            mouseFollowRenderer.transform.position = positionOnGrid;
        }

        private void OnMouseDown()
        {
            var mouseInWorld = ABenjUtils.Mouse.GetWorldPosition(Camera.main);
            var positionOnGrid = _grid.SnapToGridInWorld(mouseInWorld, 1, 1);
        }
    }
}