using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float jumpHeight = 2f;
    public float jumpTimeout = 0.10f;
    public bool grounded = false;
    public LayerMask groundLayers;
    
    private InputAction move;
    private Vector2 direction;

    private MyControls _controls;
    private CharacterController _controller;
    private PlayerInputs _input;

    private float _gravity = Physics.gravity.y;
    private float _speed;
    private float _verticalVelocity;
    private float _terminalVelocity = 56.0f;
    private float _jumpTimeoutDelta;

    public void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new MyControls();
        }
        _controls.Player.Enable();
        move = _controls.Player.Move;
    }

    public void OnDisable()
    {
        _controls.Player.Disable();
    }

    private void Start()
    {
        _jumpTimeoutDelta = jumpTimeout;
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInputs>();
    }

    private void Update()
    {
        Move();
        Jump();
        GroundedCheck();
    }

    private void Move()
    {
        _controller.Move(new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }

    private void Jump()
    {
        if (grounded)
        {
            // set velocity to make the player jump
            if (_input.jump && _jumpTimeoutDelta <= 0.0f)
            {
                _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * _gravity);
            }
            
            // reset velocity when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = _gravity;
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = jumpTimeout;

            // makes impossible to jump if not grounded
            _input.jump = false;
        }
        
        // reduces the velocity over time to make the player fall back down
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += _gravity * Time.deltaTime;
        }
    }

    private void GroundedCheck()
    {
        var spherePosition = new Vector3(transform.position.x, transform.position.y - _controller.skinWidth * 1.5f, transform.position.z);
        // use half of the height to check the bottom of the model
        grounded = Physics.CheckSphere(spherePosition, _controller.height/2, groundLayers, QueryTriggerInteraction.Ignore);
    }
}
