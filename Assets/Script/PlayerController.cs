using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerInput _input;
    Rigidbody rb;

    [Header("Speed")]
    [SerializeField] float speed = 150f;
    [SerializeField] float runningSpeed;
    [SerializeField] float runMultiplier;

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

    [Header("Rotation")]
    [Range(0.0f, 0.3f)]
    [SerializeField] float RotationSmoothTime = 0.12f;

    [Header("Jump")]
    [SerializeField] float jumpForce = 250f;
    [SerializeField] bool isGrounded = false;
    [SerializeField] float isGroundedVerifier_Height = 0.5f;
    [SerializeField] float isGroundedVerifier_Radius = 0.5f;

    [Header("Crouch")]
    [SerializeField] GameObject capsule;
    [SerializeField] GameObject sphere;
    [SerializeField] bool isUnder = false;
    [SerializeField] float isUnderVerifier_Height = 0.5f;
    [SerializeField] float isUnderVerifier_Radius = 0.4f;
    [SerializeField] LayerMask layerMask;

    [Header("Interact")]
    [SerializeField] float interactDistance = 3f;
    [SerializeField] float interactArea = 0.2f;
    [SerializeField] Vector3 interactHit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInput>();
    }
    void FixedUpdate()
    {
        Jump();
        Crouch();
        Interact();
        Move();
    }
    void LateUpdate()
    {
        CameraRotation();
    }

    //Movement
    private void Move()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 direction = cameraForward * _input.direction.z + cameraRight * _input.direction.x;

        float dotProduct = Vector3.Dot(transform.forward, direction);
        runMultiplier = Mathf.Lerp(1.25f, 2f, Mathf.InverseLerp(-0.5f, 1f, dotProduct));
        Debug.Log(dotProduct);
        runningSpeed = speed * runMultiplier;

        float speedType = _input.isRunning ? runningSpeed : speed;
        Vector3 move;
        move = new Vector3(direction.x * speedType * Time.deltaTime, rb.velocity.y, direction.z * speedType * Time.deltaTime);
        rb.velocity = move;
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
    public void Interact()
    {
        if (_input.isInteracting)
        {
            RaycastHit hitInfo;
            if (Physics.SphereCast(Camera.main.transform.position, interactArea, Camera.main.transform.forward, out hitInfo, interactDistance, layerMask))
            {
                interactHit = hitInfo.point;
            }
        }
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

        //Interact Hit
        if (interactHit != null)
        {
            Gizmos.DrawSphere(interactHit, interactArea);
        }
    }
}
