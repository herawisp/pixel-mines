using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class Player : MonoBehaviour {

    //================================================================================================//
    //================================================================================================//
    
    public float WalkSpeed;
    public float JumpPower;

    public Transform GroundCheckPoint;
    public LayerMask GroundLayer;
    public Pickaxe Pickaxe;

    Vector2 _lastDirection;
    Vector2 _direction;
    bool _isGrounded;

    Animator _animator;
    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rigidBody;

    //================================================================================================//
    //================================================================================================//

    void Awake() {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.simulated = false;
    }

    void Update() {
        _isGrounded = Physics2D.OverlapCircle(GroundCheckPoint.position, 0.2f, GroundLayer);
        _animator.SetBool("IsGrounded", _isGrounded);
        
        if (!_isGrounded) {
            _animator.SetBool("IsJumping", _rigidBody.linearVelocityY > 0f);
            _animator.SetBool("IsFalling", _rigidBody.linearVelocityY < -0f);
        } else {
            _animator.SetBool("IsJumping", false);
            _animator.SetBool("IsFalling", false);
        }

        _animator.SetBool("IsRunning", _direction.magnitude != 0);
        _spriteRenderer.flipX = _lastDirection.x > 0;
        Pickaxe.FlipX(_lastDirection.x > 0);
    }

    void FixedUpdate() {
        _rigidBody.linearVelocity = new(_direction.x * WalkSpeed, _rigidBody.linearVelocityY);
    }

    //================================================================================================//
    //================================================================================================//

    void OnMove(InputValue value) {
        _direction = value.Get<Vector2>();
        if (_direction.magnitude != 0) _lastDirection = _direction;
    }

    void OnJump() {
        if (!_isGrounded) return;
        _rigidBody.linearVelocity = new(_rigidBody.linearVelocityX, JumpPower);
    }
    
    //================================================================================================//
    //================================================================================================//

    public void Initialize(Vector3 position) {
        _rigidBody.simulated = true;
        transform.position = position; 
    }
    
    //================================================================================================//
    //================================================================================================//
}
