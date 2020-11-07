using System;
using System.Collections.Generic;
using System.Linq;
using MStudios;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ships
{
    [RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class Ship : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _boxCollider;
        
        [SerializeField] private GameObject burningEffect;
        [SerializeField] private Cannon cannonPrefab;

        private Cannon upperCannon;
        private Cannon lowerCannon;

        private Vector2 _centerOffset;
        
        private List<Vector2Int> deadCells = new List<Vector2Int>();
        
        private bool _isSelectionEnabled = false;
        private bool _isFriendly;
        public event Action<Ship> OnShipSelected;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _boxCollider = GetComponent<BoxCollider2D>();
        }


        public void FireAt(Vector2 worldPosition)
        {
            var isToTheLeft = transform.position.x > worldPosition.x;

            var upperCannonDistance = Vector3.Distance(upperCannon.transform.position, worldPosition);
            var lowerCannonDistance = Vector3.Distance(lowerCannon.transform.position, worldPosition);

            var cannon = upperCannonDistance < lowerCannonDistance ? upperCannon : lowerCannon;
            cannon.FireAt(worldPosition);
        }

        public void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        
            _boxCollider.size = sprite.bounds.size;
            _boxCollider.offset = sprite.bounds.center;

            _centerOffset = sprite.CenterOffset();
            
            CreateShipCannons(Mathf.RoundToInt(sprite.bounds.size.y), Mathf.RoundToInt(sprite.bounds.size.x));
        }

        private void CreateShipCannons(int length, int width)
        {
            lowerCannon = Instantiate(cannonPrefab, transform, true);
            lowerCannon.transform.Rotate(Vector3.forward * 180);
            lowerCannon.transform.localPosition = _centerOffset;
            lowerCannon.HideBody();


            upperCannon = Instantiate(cannonPrefab, transform, true);
            upperCannon.transform.Rotate(Vector3.forward * 180);
            upperCannon.transform.localPosition = _centerOffset + new Vector2(0,1);
            upperCannon.HideBody();
        }

        public void SetDeadCell(Vector2Int offset)
        {
            deadCells.Add(offset);
        }

        public void SetAsFriendly()
        {
            _isFriendly = true;
            upperCannon.SetAsFriendly();
            lowerCannon.SetAsFriendly();
        }

        public bool IsFriendly()
        {
            return _isFriendly;
        }
    }
}
