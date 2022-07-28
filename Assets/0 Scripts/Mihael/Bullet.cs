using System;
using UnityEngine;

public class Bullet : MonoBehaviour { 
    // inspector assigned
    [SerializeField] private int _speed = 5;
    [SerializeField] private int _timeToLive = 10;
    [SerializeField] private ParticleSystem _impactEffect;

    // internal variables
    private Rigidbody _rb;
    private Collider _collider;
    private BulletsPooling _bulletsPool;
    private float _timeAlive;
    
    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _bulletsPool = GameObject.Find("BulletsPool").GetComponent<BulletsPooling>();
    }
    
    private void FixedUpdate() {
        // fire bullet
        _rb.velocity = _speed * _rb.velocity.normalized;
        _timeAlive += Time.deltaTime;
        
        if (_timeAlive >= _timeToLive) {
            _bulletsPool.ReturnBullet(_collider.GetInstanceID());
        }
    }

    private void OnEnable() {
        if (!_rb) return;
        
        _rb.velocity = transform.forward * _speed;
        _timeAlive = 0f;
    }

    private void OnDisable() {
        if (!_rb) return;
        
        _rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision) {
        // disable components
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        // stop bullet
        _rb.velocity = Vector3.zero;
        // play particle effect
        _impactEffect.Play();
        // disable bullet with delay
        _bulletsPool.ReturnBullet(_collider.GetInstanceID(), 2);
    }
}