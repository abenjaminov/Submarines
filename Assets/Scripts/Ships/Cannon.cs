using System;
using MStudios;
using UnityEditor;
using UnityEngine;

namespace Ships
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private ParticleSystem canonFireEffect;
        [SerializeField] private CannonBall cannonBallPrefab;

        private SpriteRenderer spriteRenderer;
        private bool _isFriendly;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void FireAt(Vector2 worldPosition)
        {
            var worldPosInRefFrame = worldPosition.AsVector3() - transform.position;
            float z = Mathf.Atan2(worldPosInRefFrame.y, worldPosInRefFrame.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, z);
            
            canonFireEffect.Play(true);
            var cannonBall = Instantiate(cannonBallPrefab, transform.position, Quaternion.identity);
            if (_isFriendly)
            {
                cannonBall.SetAsFriendly();
            }
                
            cannonBall.SetTarget(worldPosition);
            cannonBall.AddForce(transform.right, 30);
        }

        public void HideBody()
        {
            spriteRenderer.enabled = false;
        }

        public void SetAsFriendly()
        {
            _isFriendly = true;
        }

        public bool IsFriendly()
        {
            return _isFriendly;
        }
    }
}
