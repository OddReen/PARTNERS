using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerInput _input;
    Rigidbody rb;

    [Header("Speed")]
    [SerializeField] float speed;
    [SerializeField] float runningSpeed;

    [Header("Camera")]
    [SerializeField] Transform cinemachineCameraTarget;
    [SerializeField] Transform cinemachineCameraCrouchTarget;
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
    public float RotationSpeed = 1.0f;
    public float TopClamp = 90.0f;
    public float BottomClamp = -90.0f;

    [Header("Rotation")]
    [Range(0.0f, 0.3f)]
    [SerializeField] float RotationSmoothTime = 0.12f;

    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] bool isGrounded;
    [SerializeField] float isGroundedVerifier_Height;
    [SerializeField] float isGroundedVerifier_Radius;

    [Header("Crouch")]
    [SerializeField] GameObject capsule;
    [SerializeField] GameObject sphere;
    [SerializeField] bool isUnder;
    [SerializeField] float isUnderVerifier_Height;
    [SerializeField] float isUnderVerifier_Radius;
    [SerializeField] LayerMask layerMask;

    [Header("Interact")]
    [SerializeField] float interactDistance;
    [SerializeField] float interactArea;
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
        Vector3 move;
        float speedType = _input.isRunning ? runningSpeed : speed;
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
            cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
            cinemachineCameraCrouchTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
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
            cinemachineCamera.Follow = cinemachineCameraCrouchTarget;
        }
        else if (!isUnder)
        {
            //Uncrouch
            capsule.SetActive(true);
            sphere.SetActive(false);
            cinemachineCamera.Follow = cinemachineCameraTarget;
        }
    }
    public bool IsUnder()
    {
        RaycastHit hitInfo;
        isUnder = Physics.SphereCast(cinemachineCameraCrouchTarget.position, isUnderVerifier_Radius, transform.up, out hitInfo, isUnderVerifier_Height, layerMask);
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + -transform.up * isGroundedVerifier_Height, isGroundedVerifier_Radius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.up * isUnderVerifier_Height, isUnderVerifier_Radius);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * interactDistance);
        if (interactHit != null)
        {
            Gizmos.DrawSphere(interactHit, interactArea);
        }
    }
}
