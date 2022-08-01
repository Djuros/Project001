using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public MyMPRef myMp;
    // Inspector assigned
    [Header("Movement")]
    [SerializeField] [Range(1, 20)] int _walkSpeed = 10;
    [SerializeField] [Range(1, 20)] private int _runSpeed = 20;
    [SerializeField] private int _rotateSpeed = 5;
    [SerializeField] private float _jumpForce = 4f;

    [Header("Shooting")] 
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] AudioSource _as;
    [SerializeField] AudioClip fire_clip;
    public Transform muzzle;
    // Internal variables
    private CharacterController _controller;
    private Camera _camera;
    public Animator _anim;
    private Player _player;
    private float _shootCounter;
    private float _yForce;

    // animation parameter hashes
    public readonly int _runParameterHash = Animator.StringToHash("Run");
    public readonly int _shootParameterHash = Animator.StringToHash("Shoot");
    public readonly int _fireRateParameterHash = Animator.StringToHash("FireRate");
    public readonly int _deathParameterHash = Animator.StringToHash("Death");
    public readonly int _jumpParameterHash = Animator.StringToHash("Jump");

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
        // dont move if dead
        if (myMp.dead) return;
        
        // get input
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        MovePlayer(horizontal, vertical);
        RotatePlayerToMousePosition();

        if (Input.GetButton("Fire1") && _controller.isGrounded) ShootBullet();
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
        var desiredMove = transform.forward * input.y + transform.right * input.x;
        // set movement speed
        var running = Input.GetButton("Run");
        desiredMove = running ? desiredMove * _runSpeed : desiredMove * _walkSpeed;

        // apply gravity
        _yForce += Physics.gravity.y * Time.deltaTime;
        
        // jump if needed
        if (Input.GetButtonDown("Jump") && _controller.isGrounded) {
            _anim.SetTrigger(_jumpParameterHash);
            _yForce = Mathf.Sqrt(_jumpForce * 2 * -Physics.gravity.y);
            MessagingSystem.ins.I_Jump(myMp.pv.ViewID);
        }
        
        // fall at most at the force of gravity
        desiredMove.y = Mathf.Max(Physics.gravity.y, _yForce);
        
        // move with character controller
        _controller.Move(desiredMove * Time.deltaTime);
        // activate run animation based on the input
        _anim.SetFloat(_runParameterHash, running ? 2f * input.magnitude : input.magnitude);
    }
    
    private void ShootBullet() {
        if (_shootCounter < 1f) return;
        _shootCounter = 0f;
        
        _anim.SetTrigger(_shootParameterHash);
        // set fire animation speed
        _anim.SetFloat("FireRate", _fireRate);
    }

    // public methods
    public void DeathAnimation() {
        _anim.SetTrigger(_deathParameterHash);
    }
    
    // public callback methods
    
    // this method is called when the animation fires the bullet
    public void ShootCallback() {
        if (myMp.dead) { return; }
        // setup bullet instantiation
        _as.volume = 0.2f;
        _as.PlayOneShot(fire_clip);
        BulletPool.ins.Fire();
        MessagingSystem.ins.I_Fire(myMp.pv.ViewID,0);
    }
}