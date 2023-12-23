using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MultiplayerPlayerController : NetworkBehaviour
{
    public static MultiplayerPlayerController OwnerInstance;

    MultiplayerPlayerInput _input;
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
    //[SerializeField] float speedDebuffMultiplier = .75f;
    [SerializeField] float runMultiplier = 2f;
    [Range(0, 1)]
    [SerializeField] float crouchMultiplier = .5f;

    [Header("Camera")]
    [SerializeField] Transform cameraTarget;
    [SerializeField] Transform cameraCrouchTarget;
    [SerializeField] PlayerCamera playerCameraPrefab;
    Camera playerCamera;
    CinemachineVirtualCamera cinemachineCamera;

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
    GameObject interactHint;
    public Transform grabPosition;

    GameObject hitInfoGameObject;

    bool isSpawned = false;
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            rb = GetComponent<Rigidbody>();
            _input = GetComponent<MultiplayerPlayerInput>();
            _input.InteractAction += PlayerInput_InteractAction;
            PlayerCamera camera = Instantiate(playerCameraPrefab);
            playerCamera = camera.playerCamera;
            cinemachineCamera = camera.virtualCamera;
            interactHint = AssetsReference.Instance.interactionHint;
            OwnerInstance = this;
            isSpawned = true;
        }
    }

    void FixedUpdate()
    {
        //Se não for o dono de este object não executar o codigo
        if (!IsOwner && isSpawned) return;

        MoveStates();
        Jump();
        Crouch();
        InteractHint();
        Move();
    }
    void LateUpdate()
    {
        if (!IsOwner && isSpawned) return;

        CameraRotation();
    }

    //Movement
    private void Move()
    {
        //Camera Relative Direction
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 direction = cameraForward * _input.direction.z + cameraRight * _input.direction.x;

        //Dot Product of Foward and Direction
        float dotProduct = Vector3.Dot(transform.forward, direction);
        float currentSpeed = Mathf.Lerp(speed * 0.5f, speed, Mathf.InverseLerp(-0.5f, 1f, dotProduct));

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
    //Todo ask leo if this is better has a event?
    public void Jump()
    {
        if (IsGrounded() && _input.isJumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce);
        }
    }

    #region IsChecks
    public bool IsGrounded()
    {
        //If seen tell nardo 2 that for some fucking reason the spherecast wasnt working so switched to raycast
        isGrounded = Physics.Raycast(transform.position, -transform.up,isGroundedVerifier_Height, layerMask);
        return isGrounded;
    }

    public bool IsUnder()
    {
        RaycastHit hitInfo;
        isUnder = Physics.SphereCast(cameraCrouchTarget.position, isUnderVerifier_Radius, transform.up, out hitInfo, isUnderVerifier_Height, layerMask);
        return isUnder;
    }
    #endregion

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

    #region Interaction
    private void PlayerInput_InteractAction(object sender, System.EventArgs e)
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, interactDistance, layerMask))
        {
            if (hitInfo.collider.CompareTag("Interactable"))
            {
                //Estava a dar compile error
                //hitInfo.collider.GetComponent<Interactable>().Interact();
            }
        }
    }
    public void InteractHint()
    {
        bool hasInteractHint;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, interactDistance, layerMask))
        {
            if (hitInfo.collider.CompareTag("Interactable"))
            {
                if (hitInfoGameObject != null)
                {
                    hitInfoGameObject.GetComponent<Interactable>().InteractHint(false);
                }
                hitInfoGameObject = hitInfo.collider.gameObject;
                hasInteractHint = true;
            }
            else
            {
                hasInteractHint = false;
            }
        }
        else
        {
            hasInteractHint = false;
        }
        if (hitInfoGameObject != null)
        {
            hitInfoGameObject.GetComponent<Interactable>().InteractHint(hasInteractHint);
        }
    }
    #endregion
}
