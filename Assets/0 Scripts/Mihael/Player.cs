using System;
using UnityEngine;

public class Player : MonoBehaviour{
    // Inspector assigned
    [Header("General")] 
    [SerializeField] private int _health = 100;

    // public fields
    public int Health => _health;

    // Internal variables
    private PlayerController _playerController;

    // Unity event methods
    private void Start() {
        _playerController = GetComponent<PlayerController>();
    }
    
    private void Update() {
        
    }

    // private methods
    
    
    // public methods

    // callbacks
}