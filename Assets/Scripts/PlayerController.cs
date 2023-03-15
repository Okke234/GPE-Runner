using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Running vars")]
    public float forwardSpeed = 5f;
    public float lateralSpeed = 100f;
    // could be dynamic for difficulty increase
    public float forwardTargetVelocity = 20f;
    public float lateralTargetVelocity = 2f;
    public bool running = true;
    [Space(10)]
    [Header("Jumping vars")]
    public float jumpHeight = 2f;
    public float jumpTimeout = 0.10f;
    public bool grounded = false;
    public LayerMask groundLayers;
    
    private MyControls _controls;
    private CharacterController _controller;
    private PlayerInputs _input;

    private readonly float _gravity = 2 * Physics.gravity.y;
    private const float TerminalVelocity = 56.0f;
    private float _forwardVelocity;
    private float _lateralVelocity;
    private float _verticalVelocity;
    private float _jumpTimeoutDelta;

    public void OnEnable()
    {
        _controls ??= new MyControls();
        _controls.Player.Enable();
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
        
        // Test cubes
        for (var i = 0; i < 50; i++)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(2.0f, 1.0f, 10f * i);
        }
    }

    private void Update()
    {
        Move();
        Jump();
        GroundedCheck();
    }

    private void Move()
    {
        // constant forward movement
        if (running)
        {
            // gradually increase running speed (at the start) until target speed has been reached
            if (_forwardVelocity < forwardTargetVelocity)
            {
                _forwardVelocity += forwardSpeed * Time.deltaTime;
            }
            else
            {
                _forwardVelocity = forwardTargetVelocity;
            }

            if (_input.moving)
            {
                // gradually increase lateral speed when button is pressed, until target speed has been reached
                if (Mathf.Abs(_lateralVelocity) < lateralTargetVelocity)
                {
                    _lateralVelocity += _input.move.x * lateralSpeed * Time.deltaTime;
                }
                else
                {
                    _lateralVelocity = _input.move.x * lateralTargetVelocity;
                }
            }
            else
            {
                // reset lateral speed if no button is being pressed
                _lateralVelocity = 0.0f;
            }
        }

        _controller.Move(new Vector3(_lateralVelocity, _verticalVelocity, _forwardVelocity) * Time.deltaTime);
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
        if (_verticalVelocity < TerminalVelocity)
        {
            _verticalVelocity += _gravity * Time.deltaTime;
        }
    }

    private void GroundedCheck()
    {
        // the controller's skin width pushes the player up a little; use it, with a little extra leeway, as an offset
        var spherePosition = new Vector3(transform.position.x, transform.position.y - _controller.skinWidth * 1.5f, transform.position.z);
        // use half of the height to check the bottom of the model
        grounded = Physics.CheckSphere(spherePosition, _controller.height/2, groundLayers, QueryTriggerInteraction.Ignore);
    }
}
