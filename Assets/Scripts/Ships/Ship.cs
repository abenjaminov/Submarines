using MStudios;
using UnityEngine;

namespace Ships
{
    [RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class Ship : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _boxCollider;

        [SerializeField] private Sprite deadCellSprite;
    
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _boxCollider = GetComponent<BoxCollider2D>();
        }

        public void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        
            _boxCollider.size = sprite.bounds.size;
            _boxCollider.offset = sprite.bounds.center;
        }

        public void SetDeadCell(Vector2Int offset)
        {
            MUtils.CreateSpriteObject2D(transform, offset, deadCellSprite, Color.white,_spriteRenderer.sortingOrder + 1);
        }
    }
}
