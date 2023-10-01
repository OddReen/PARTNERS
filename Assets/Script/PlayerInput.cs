using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInput : MonoBehaviour
{
    InputController inputController;

    public Vector3 direction;
    public Vector2 look;

    public bool isRunning;
    public bool isJumping;
    public bool isCrouching;
    public bool isInteracting;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        inputController = new InputController();
        inputController.Gameplay.Enable();
    }
    private void OnEnable()
    {
        inputController.Gameplay.Move.performed += MoveInput;
        inputController.Gameplay.Move.canceled += MoveInput;
        inputController.Gameplay.Look.performed += LookInput;
        inputController.Gameplay.Look.canceled += LookInput;
        inputController.Gameplay.Run.performed += RunInput;
        inputController.Gameplay.Run.canceled += RunInput;
        inputController.Gameplay.Jump.performed += JumpInput;
        inputController.Gameplay.Jump.canceled += JumpInput;
        inputController.Gameplay.Crouch.performed += CrouchInput;
        inputController.Gameplay.Crouch.canceled += CrouchInput;
        inputController.Gameplay.Interaction.performed += InteractionInput;
        inputController.Gameplay.Interaction.canceled += InteractionInput;
    }
    private void OnDisable()
    {
        inputController.Gameplay.Move.performed -= MoveInput;
        inputController.Gameplay.Move.canceled -= MoveInput;
        inputController.Gameplay.Look.performed -= LookInput;
        inputController.Gameplay.Look.canceled -= LookInput;
        inputController.Gameplay.Run.performed -= RunInput;
        inputController.Gameplay.Run.canceled -= RunInput;
        inputController.Gameplay.Jump.performed -= JumpInput;
        inputController.Gameplay.Jump.canceled -= JumpInput;
        inputController.Gameplay.Crouch.performed -= CrouchInput;
        inputController.Gameplay.Crouch.canceled -= CrouchInput;
        inputController.Gameplay.Interaction.performed -= InteractionInput;
        inputController.Gameplay.Interaction.canceled -= InteractionInput;
    }
    public void MoveInput(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        direction = new Vector3(moveInput.x, 0f, moveInput.y);
    }
    public void LookInput(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }
    public void RunInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            isRunning = true;
        else if (context.canceled)
            isRunning = false;
    }
    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            isJumping = true;
        else if (context.canceled)
            isJumping = false;
    }
    public void CrouchInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            isCrouching = true;
        else if (context.canceled)
            isCrouching = false;
    }
    public void InteractionInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            isInteracting = true;
        else if (context.canceled)
            isInteracting = false;
    }
}