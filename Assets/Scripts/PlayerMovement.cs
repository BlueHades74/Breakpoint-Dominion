using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Input Things")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 12f;
    private Rigidbody rb;

    [SerializeField] private GameObject cam;

    private Vector2 lookInput;
    [SerializeField] private float lookSensitivity = 0.1f;
    private Vector2 moveInput;
    private Vector3 move;
    [SerializeField] private bool isGrounded = true;

    private float maxHealth = 250f;
    [SerializeField] private NetworkVariable<float> health;
    private bool canTakeDamage = true;
    [SerializeField] private float damageInvulTime = 1f;
    [SerializeField] private HealthBar healthBar;

    public override void OnNetworkSpawn()
    {
        if (IsServer) health.Value = maxHealth;

        if (IsOwner)
        {
            cam.SetActive(true);
            healthBar.UpdateHealthBar(health.Value, maxHealth);
            // need to set personal health bar to inactive
        }
    }

    private void OnEnable()
    {
        PlayerGroundedCheck.onTouchingGround += GroundedTrue;
    }

    private void OnDisable()
    {
        PlayerGroundedCheck.onTouchingGround -= GroundedTrue;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (IsOwner)
        {
            GetComponent<Renderer>().material.color = Color.blue;
            rb = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        
        float mouseX = lookInput.x * lookSensitivity;
        // rotate player based on mouse position
        transform.Rotate(Vector3.up * mouseX);
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        if (moveInput.magnitude >= 0.1)
        {
            // set the movement to the current linear velocity
            move = rb.linearVelocity;

            // grab the angle we are at to move forward
            float targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + transform.eulerAngles.y;
            // set the move direction
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            moveDir *= moveSpeed;

            // reset the y velocity
            moveDir.y = move.y;

            // set the linear velocity to the new values
            rb.linearVelocity = moveDir;
        }
        else
        {
            // stop the player from moving without input (no sliding)
            move = rb.linearVelocity;
            // keep the gravity on though
            move.x = 0f;
            move.z = 0f;

            rb.linearVelocity = move;
        }
    }

    // Movement Code
    #region
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
            var moveY = rb.linearVelocity;
            moveY.y = jumpPower;
            rb.linearVelocity = moveY;
            isGrounded = false;
        }
    }

    public void Look(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

        lookInput = context.ReadValue<Vector2>();
    }

    private void GroundedTrue()
    {
        if (!IsOwner) return;

        isGrounded = true;
    }
    #endregion

    public void TakeDamage(float damageRecieved)
    {
        if (canTakeDamage)
        {
            health.Value -= damageRecieved;
            Debug.Log($"Player health: {health.Value}");
            healthBar.UpdateHealthBar(health.Value, maxHealth);
            StartCoroutine(DamageInvulnerability());
            canTakeDamage = false;
        }
    }

    private IEnumerator DamageInvulnerability()
    {
        yield return new WaitForSeconds(damageInvulTime);
        canTakeDamage = true;
    }
}
