using System;
using System.Collections;
using System.Collections.Generic;
using MStudios.Events.GameEvents;
using Ships;
using UnityEngine;
using UnityEngine.Events;

public class CannonBall : MonoBehaviour
{
    private Rigidbody2D cannonRb;
    [SerializeField] private float destructionTime;
    [SerializeField] private CannonBallHitTargetGameEvent cannonBallHitTarget;

    private bool hasTarget;
    private Vector3 target;
    private float aliveTime;

    private bool _isFriendly;
    
    private void Awake()
    {
        cannonRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        aliveTime += Time.deltaTime;
        destructionTime -= Time.deltaTime;

        if (destructionTime <= 0)
        {
            Destroy(gameObject);
        }

        if (!hasTarget) return;
        
        if (Vector3.Distance(transform.position, target) < 0.3f)
        {
            this.cannonBallHitTarget.Raise(new CannonBallTargetHitInfo()
            {
                cannonBall = this,
                target = this.target
            });
        }
    }

    public void AddForce(Vector2 direction, float force)
    {
        cannonRb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public void SetTarget(Vector2 worldTarget)
    {
        target = worldTarget;
        hasTarget = true;
    }

    public float GetTimeAlive()
    {
        return aliveTime;
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
