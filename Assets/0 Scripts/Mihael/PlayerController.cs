using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Inspector assigned
    [Header("Movement")]
    [SerializeField] [Range(1, 20)] int _moveSpeed = 10;
    [SerializeField] private float _stickToGrouondForce = 5f;
    [SerializeField] private int _rotateSpeed = 5;
    
    // Internal variables
    private CharacterController _controller;
    private Camera _camera;
    private Animator _anim;
    private Player _player;
    
    // animation parameter hashes
    private readonly int _runParameterHash = Animator.StringToHash("Run");
    private readonly int _shootParameterHash = Animator.StringToHash("Shoot");

    // Unity event methods
    private void Start() {
        // get components
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        _player = GetComponent<Player>();
        _camera = Camera.main;
    }
    
    private void FixedUpdate() {
        // get input
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        
        MovePlayer(horizontal, vertical);
        RotatePlayer();
        
        if (Input.GetButton("Fire1")) _player.Shoot();
    }

    // Private methods
    private void RotatePlayer() {
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
        desiredMove *= _moveSpeed;
        
        // put down pressure, so the player stick to the ground
        desiredMove.y -= _stickToGrouondForce;
        // move with character controller
        _controller.Move(desiredMove * Time.deltaTime);
        // activate run animation based on the input
        _anim.SetFloat(_runParameterHash, input.magnitude);
    }
    
    // public methods
    public void ShootAnimation() {
        _anim.SetTrigger(_shootParameterHash);
    }
}