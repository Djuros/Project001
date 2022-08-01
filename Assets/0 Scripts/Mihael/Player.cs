using System;
using UnityEngine;

public class Player : MonoBehaviour{
    // Inspector assigned
    [Header("General")] 
    [SerializeField] [Range(0,100)] private int _health = 100;
    [SerializeField] private int _deathDelay = 5;

    // public fields
    public int Health => _health;
    public bool Dead => _health <= 0;

    // Internal variables
    private PlayerController _playerController;

    // Unity event methods
    private void Start() {
        _playerController = GetComponent<PlayerController>();
    }
    
    private void Update() {
        // only for debugging
        if (_health <= 0) Die();
    }

    // private methods
    private void Die() {
        // destroy game object with delay
        Destroy(gameObject, _deathDelay);
        _playerController.DeathAnimation();
    }
    
    
    // public methods
    public void TakeDamage(int damage) {
        _health -= damage;
        if (_health <= 0) {
            Die();
        }
    }

    // callbacks
}