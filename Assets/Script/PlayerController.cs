using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerInput _input;
    Rigidbody rb;

    [Header("Movement States")]
    [SerializeField] MovementState movementState;
    enum MovementState
    {
        Idle,
        Walk,
        Run,
        Crouch
    }

    [Header("Speed")]
    [SerializeField] float speed = 150f;
    [Range(0, 1)]
    [SerializeField] float speedDebuffMultiplier = .75f;
    [SerializeField] float runMultiplier = 2f;
    [Range(0, 1)]
    [SerializeField] float crouchMultiplier = .5f;

    [Header("Camera")]
    [SerializeField] Transform cameraTarget;
    [SerializeField] Transform cameraCrouchTarget;
    [SerializeField] CinemachineVirtualCamera cinemachineCamera;

    float _cinemachineTargetPitch;
    const float _threshold = 0.01f;
    private float _rotationVelocity;
    private bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
				return _playerInput.currentControlScheme == "KeyboardMouse";
#else
            return false;
#endif
        }
    }
    public float RotationSpeed = 50f;
    public float TopClamp = 85f;
    public float BottomClamp = -85f;

    [Header("Jump")]
    [SerializeField] float jumpForce = 250f;
    [SerializeField] bool isGrounded = false;
    [SerializeField] float isGroundedVerifier_Height = 0.5f;
    [SerializeField] float isGroundedVerifier_Radius = 0.5f;

    [Header("Crouch")]
    [SerializeField] GameObject capsule;
    [SerializeField] GameObject sphere;
    [SerializeField] bool isUnder = false;
    [SerializeField] float isUnderVerifier_Height = 1f;
    [SerializeField] float isUnderVerifier_Radius = 0.4f;
    [SerializeField] LayerMask layerMask;

    [Header("Interact")]
    [SerializeField] float interactDistance = 3f;
    [SerializeField] GameObject interactHint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInput>();
    }
    private void OnEnable()
    {
        PlayerInput.DoInteract += Interact;
        PlayerInput.StopInteract += Interact;
    }
    private void OnDisable()
    {
        PlayerInput.DoInteract -= Interact;
        PlayerInput.StopInteract -= Interact;
    }
    void FixedUpdate()
    {
        MoveStates();
        Jump();
        Crouch();
        InteractHint();
        Move();
    }
    void LateUpdate()
    {
        CameraRotation();
    }

    //Movement
    private void Move()
    {
        //Camera Relative Direction
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 direction = cameraForward * _input.direction.z + cameraRight * _input.direction.x;

        //Dot Product of Foward and Direction
        float dotProduct = Vector3.Dot(transform.forward, direction);
        float currentSpeed = Mathf.Lerp(speed * speedDebuffMultiplier, speed, Mathf.InverseLerp(-0.5f, 1f, dotProduct));

        switch (movementState)
        {
            case MovementState.Crouch:
                currentSpeed *= crouchMultiplier;
                break;
            case MovementState.Run:
                currentSpeed *= runMultiplier;
                break;
            default:
                break;
        }
        rb.velocity = new Vector3(direction.x * currentSpeed * Time.deltaTime, rb.velocity.y, direction.z * currentSpeed * Time.deltaTime);
    }
    private void MoveStates()
    {
        if (_input.isCrouching || IsUnder())
        {
            movementState = MovementState.Crouch;
        }
        else if (_input.isRunning)
        {
            movementState = MovementState.Run;
        }
        else if (_input.direction != Vector3.zero)
        {
            movementState = MovementState.Walk;
        }
        else
        {
            movementState = MovementState.Idle;
        }
    }

    //Camera
    private void CameraRotation()
    {
        if (_input.look.sqrMagnitude >= _threshold)
        {
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetPitch += -_input.look.y * RotationSpeed * deltaTimeMultiplier;
            _rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
            cameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
            cameraCrouchTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
            transform.Rotate(Vector3.up * _rotationVelocity);
        }
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    //Jump
    public void Jump()
    {
        if (IsGrounded() && _input.isJumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce);
        }
    }
    public bool IsGrounded()
    {
        RaycastHit hitInfo;
        isGrounded = Physics.SphereCast(transform.position, isGroundedVerifier_Radius, -transform.up, out hitInfo, isGroundedVerifier_Height, layerMask);
        return isGrounded;
    }

    //Crouch
    public void Crouch()
    {
        IsUnder();
        if (_input.isCrouching)
        {
            //Crouch
            capsule.SetActive(false);
            sphere.SetActive(true);
            cinemachineCamera.Follow = cameraCrouchTarget;
        }
        else if (!isUnder)
        {
            //Uncrouch
            capsule.SetActive(true);
            sphere.SetActive(false);
            cinemachineCamera.Follow = cameraTarget;
        }
    }
    public bool IsUnder()
    {
        RaycastHit hitInfo;
        isUnder = Physics.SphereCast(cameraCrouchTarget.position, isUnderVerifier_Radius, transform.up, out hitInfo, isUnderVerifier_Height, layerMask);
        return isUnder;
    }

    //Interact
    public void InteractHint()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, interactDistance, layerMask))
        {
            //PlaceHolder
            switch (hitInfo.collider.tag)
            {
                case "MusicBox":
                    interactHint.SetActive(true);
                    break;
                case "Console":
                    interactHint.SetActive(true);
                    break;
                case "Door":
                    interactHint.SetActive(true);
                    break;
                default:
                    interactHint.SetActive(false);
                    break;
            }
        }
        else
        {
            interactHint.SetActive(false);
        }
    }
    public void Interact()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, interactDistance, layerMask))
        {
            switch (hitInfo.collider.tag)
            {
                case "Door":
                    hitInfo.collider.GetComponent<Door>().ExecuteAction();
                    break;
                case "MusicBox":
                    hitInfo.collider.GetComponent<MonsterBehaviour>().MusicBoxRestart();
                    break;
                case "Console":
                    break;
                case "Player":
                    break;
                default:
                    break;
            }
        }
    }

    //Debug
    void OnGUI()
    {
        GUILayout.Label(speed.ToString());
    }
    private void OnDrawGizmos()
    {
        //IsGrounded SphereCast
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + -transform.up * isGroundedVerifier_Height, isGroundedVerifier_Radius);

        //IsUnder SphereCast
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.up * isUnderVerifier_Height, isUnderVerifier_Radius);

        //Interact Ray Cast
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * interactDistance);
    }
}
