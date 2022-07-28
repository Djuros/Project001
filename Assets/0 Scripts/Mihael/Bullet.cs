using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour { 
    // inspector assigned
    [SerializeField] private int _speed = 5;
    [SerializeField] private int _timeToLive = 10;
    
    // internal variables
    private Rigidbody _rb;
    
    private void Start() {
        Destroy(gameObject, _timeToLive);
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = transform.forward * _speed;
    }
    
    private void FixedUpdate() {
        // fire bullet
        _rb.velocity = _speed * _rb.velocity.normalized;
    }
    
    private void OnCollisionEnter(Collision collision) {
        // destroy bullet on collision
        Destroy(gameObject);
    }
}