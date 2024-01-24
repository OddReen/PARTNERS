using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerStrafe_Multiplayer : NetworkBehaviour
{
    PlayerController_Multiplayer _playerController;
    PlayerInput_Multiplayer _playerInput;
    [SerializeField] Animator _animator;

    [Header("Animation")]
    [SerializeField] GameObject playerModel;
    [SerializeField] float currentMoveIndex;
    [SerializeField] float changeMoveIndexSpeed = 5;
    float targetMoveIndex;
    Vector3 lookAtVector = Vector3.zero;

    private void Start()
    {
        _playerController = GetComponent<PlayerController_Multiplayer>();
        _playerInput = GetComponent<PlayerInput_Multiplayer>();
    }
    private void LateUpdate()
    {
        if (!IsOwner) return;
        MoveStates();
        Animate();
    }
    private void Animate()
    {
        if (_playerInput.direction != Vector3.zero)
        {
            float dotProduct = Vector3.Dot(transform.forward, transform.rotation * _playerInput.direction);

            if (dotProduct >= 0)
            {
                //Rotation
                lookAtVector = Vector3.MoveTowards(lookAtVector, playerModel.transform.position + (transform.rotation * _playerInput.direction), Time.deltaTime * changeMoveIndexSpeed);
                playerModel.transform.LookAt(lookAtVector);
                //Forward Animation
                currentMoveIndex = Mathf.MoveTowards(currentMoveIndex, targetMoveIndex, Time.deltaTime * changeMoveIndexSpeed);
            }
            else
            {
                //Rotation
                Vector3 newDir = new Vector3();
                newDir.z = Mathf.Abs(_playerInput.direction.z);
                newDir.x = _playerInput.direction.x * -1;
                lookAtVector = Vector3.MoveTowards(lookAtVector, playerModel.transform.position + (transform.rotation * newDir), Time.deltaTime * changeMoveIndexSpeed);
                playerModel.transform.LookAt(lookAtVector);
                //Backward Animation
                currentMoveIndex = -Mathf.MoveTowards(Mathf.Abs(currentMoveIndex), targetMoveIndex, Time.deltaTime * changeMoveIndexSpeed);
            }
        }
        else
        {
            //Reset Rotation
            lookAtVector = Vector3.MoveTowards(lookAtVector, playerModel.transform.position + transform.forward, Time.deltaTime * changeMoveIndexSpeed);
            playerModel.transform.LookAt(lookAtVector);
            //Idle Animation
            currentMoveIndex = Mathf.MoveTowards(currentMoveIndex, targetMoveIndex, Time.deltaTime * changeMoveIndexSpeed);
        }
        _animator.SetBool("IsMoving", currentMoveIndex > 0.1f || currentMoveIndex < -0.1f);
        _animator.SetFloat("Move", currentMoveIndex);
    }
    private void MoveStates()
    {
        switch (_playerController.movementState)
        {
            case PlayerController_Multiplayer.MovementState.Crouch:
                break;
            case PlayerController_Multiplayer.MovementState.Run:
                targetMoveIndex = _playerInput.direction.magnitude * 2;
                break;
            case PlayerController_Multiplayer.MovementState.Walk:
                targetMoveIndex = _playerInput.direction.magnitude;
                break;
            case PlayerController_Multiplayer.MovementState.Idle:
                targetMoveIndex = 0;
                break;
            default:
                break;
        }
    }
}
