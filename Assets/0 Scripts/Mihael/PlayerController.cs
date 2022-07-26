using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Inspector assigned
    [Header("Movement")]
    [SerializeField] [Range(1, 20)] int _walkSpeed = 10;
    [SerializeField] [Range(10, 30)] private int _runSpeed = 20;
    [SerializeField] private float _stickToGrouondForce = 5f;
    [SerializeField] private int _rotateSpeed = 5;
    [Header("Shooting")] 
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Transform _bulletParent;
    [SerializeField] private float _fireRate = 1f;
    
    // Internal variables
    private CharacterController _controller;
    private Camera _camera;
    private Animator _anim;
    private Player _player;
    private float _shootCounter;
    
    // animation parameter hashes
    private readonly int _runParameterHash = Animator.StringToHash("Run");
    private readonly int _shootParameterHash = Animator.StringToHash("Shoot");
    private readonly int _fireRateParameterHash = Animator.StringToHash("FireRate");

    // Unity event methods
    private void Start() {
        // get components
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        _player = GetComponent<Player>();
        _camera = Camera.main;
    }

    private void Update() {
        _shootCounter += Time.deltaTime * _fireRate;
    }

    private void FixedUpdate() {
        // get input
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        
        MovePlayer(horizontal, vertical);
        RotatePlayerToMousePosition();

        if (Input.GetButton("Fire1")) ShootBullet();
    }

    // Private methods
    private void RotatePlayerToMousePosition() {
        // cast ray through mouse cursor
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        // only hit colliders on terrain layer
        var mask = LayerMask.GetMask("Terrain");
        
        if (Physics.Raycast(ray, out var hit, 50f, mask)) {
            var direction = hit.point - transform.position;
            var newRotation = Vector3.RotateTowards(transform.forward, direction, _rotateSpeed * Time.deltaTime, 0f);
            // only rotate around y axis
            newRotation.y = 0f;
            transform.rotation = Quaternion.LookRotation(newRotation);
        }
    }

    private void MovePlayer(float horizontal, float vertical) {
        var input = new Vector2(horizontal, vertical);
        // normalize if needed
        if (input.sqrMagnitude > 1) input.Normalize();
        
        // move along the global axis:
        var desiredMove = Vector3.forward * input.y + Vector3.right * input.x;
        // set movement speed
        var running = Input.GetButton("Run");
        desiredMove = running ? desiredMove * _runSpeed : desiredMove * _walkSpeed;

        // put down pressure, so the player stick to the ground
        desiredMove.y -= _stickToGrouondForce;
        // move with character controller
        _controller.Move(desiredMove * Time.deltaTime);
        // activate run animation based on the input
        _anim.SetFloat(_runParameterHash, running ? 2f * input.magnitude : input.magnitude);
    }
    
    // public methods
    private void ShootBullet() {
        if (_shootCounter < 1f) return;
        _shootCounter = 0f;
        
        _anim.SetTrigger(_shootParameterHash);
        // set fire animation speed
        _anim.SetFloat(_fireRateParameterHash, _fireRate);
    }
    
    // public callback methods
    
    /// <summary>
    /// this method is called when the animation fires the bullet
    /// </summary>
    public void ShootCallback() {
        // setup bullet
        Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation, _bulletParent);
    }
}