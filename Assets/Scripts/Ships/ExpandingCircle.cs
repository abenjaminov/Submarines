using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ExpandingCircle : MonoBehaviour
{
    [SerializeField]
    private float lifeTimeInSeconds;

    private SpriteRenderer _spriteRenderer;
    private Material _spriteMaterial;
    private Vector3 _localScale;
    
    private float _timeToLive;
    
    void Start()
    {
        _timeToLive = lifeTimeInSeconds;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteMaterial = _spriteRenderer.material;
        _localScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        _timeToLive -= Time.deltaTime;

        if (_timeToLive <= 0)
        {
            Destroy(gameObject);
        }

        var color = _spriteRenderer.material.color;
        
        _spriteMaterial.color = new Color(color.r, color.g,color.b, _timeToLive/ lifeTimeInSeconds);
        this.transform.localScale = new Vector3(_localScale.x + 0.01f,_localScale.y + 0.01f);
        _localScale = this.transform.localScale;
    }
}
