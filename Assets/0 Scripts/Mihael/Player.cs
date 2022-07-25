using System;
using UnityEngine;

public class Player : MonoBehaviour{
    // Inspector assigned
    [Header("General")] 
    [SerializeField] private int _health = 100;
    [Header("Shooting")] 
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Transform _bulletParent;
    [SerializeField] private float _shootPeriod = 0.1f;
    
    // public fields
    public int Health => _health;

    // Internal variables
    private PlayerController _playerController;

    private float _shootCounter = 0f;

    // Unity event methods
    private void Start() {
        _playerController = GetComponent<PlayerController>();
    }

    private void Update() {
        _shootCounter += Time.deltaTime;
    }

    // private methods
    
    
    // public methods
    public void Shoot() {
        if (_shootCounter < _shootPeriod) return;
        _shootCounter = 0f;
        
        _playerController.ShootAnimation();
        // setup bullet
        Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation, _bulletParent);
    }
}