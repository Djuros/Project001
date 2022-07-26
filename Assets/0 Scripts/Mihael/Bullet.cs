using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour { 
    // inspector assigned
    [SerializeField] private int _speed = 5;
    [SerializeField] private int _timeToLive = 10;
    
    private void Start() {
        Destroy(gameObject, _timeToLive);
    }
    
    private void Update() {
        transform.position += transform.forward * (_speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { 
         //   other.GetComponent<my>
        }
    }
}