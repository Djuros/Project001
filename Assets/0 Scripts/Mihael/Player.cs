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

    public float _shootCounter = 0f;

    // Unity event methods
    private void Start() {
        _playerController = GetComponent<PlayerController>();
    }
    public float fire_rate = 3, fire_cnt;
    public void Shoot() {
        if (fire_cnt < 1)
        {
            fire_cnt += Time.deltaTime * fire_rate;
        }
        else{
            fire_cnt = 0;
            print("FIRE");
            _shootCounter = 0f;
            _playerController._anim.speed = fire_rate;
            _playerController._anim.Play("Shoot", 1);
        }
    }
    
    // callbacks
    public void ShootCallback() {
        // setup bullet
        Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation, _bulletParent);
    }
}