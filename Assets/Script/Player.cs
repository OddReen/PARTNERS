using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerInput playerInput;
    Rigidbody rb;

    [SerializeField] float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }
    void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        rb.velocity = playerInput.direction * speed * Time.deltaTime;
    }
    private void RotateWithCamera()
    {

    }
}
