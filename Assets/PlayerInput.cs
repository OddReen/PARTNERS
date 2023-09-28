using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInput : MonoBehaviour
{
    InputController inputController;

    public Vector3 direction;
    public Vector2 look;

    private void Awake()
    {
        inputController = new InputController();
        inputController.Gameplay.Enable();
    }
    private void OnEnable()
    {
        inputController.Gameplay.Move.performed += MoveInput;
        inputController.Gameplay.Move.canceled += MoveInput;
    }
    private void OnDisable()
    {
        inputController.Gameplay.Move.performed -= MoveInput;
        inputController.Gameplay.Move.canceled -= MoveInput;
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
}