using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 12f;
    private Rigidbody rb;

    private Vector2 moveInput;
    private Vector3 move;
    private bool isGrounded = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (IsOwner)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        // set the movement to the current linear velocity
        move = rb.linearVelocity;
        // adjust the values in the x and z based on input
        move.x = moveInput.x * moveSpeed;
        move.z = moveInput.y * moveSpeed;
        // set the linear velocity to the new values
        rb.linearVelocity = move;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        // set a vector2 to the input (already normalized)
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isGrounded) return;

        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
    }
}
