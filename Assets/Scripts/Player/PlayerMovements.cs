using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 4f;
    public float sprintMultiplier = 1.5f; // Sprint speed = moveSpeed * sprintMultiplier
    public float airControlMultiplier = 0.6f; // Air movement is slower
    public float jumpMultiplier = 3.5f; // Jump force relative to moveSpeed
    public float gravityMultiplier = 25f; // Fixed gravity (doesn't scale with moveSpeed)
    public float movementForceMultiplier = 3.5f; // Scales force applied to movement

    private bool isGrounded;
    public Transform playerBody;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;

    private float coyoteTime = 0.2f;
    private float lastGroundedTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.linearDamping = moveSpeed * 0.8f; // Drag scales with moveSpeed
    }

    private void Update()
    {
        MoveInput();
        CheckGround();
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
        ApplyGravity();
    }

    private void MoveInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = (playerBody.forward * verticalInput + playerBody.right * horizontalInput).normalized;

        // Adjust movement speed dynamically
        float currentSpeed = isGrounded
            ? moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f)
            : moveSpeed * airControlMultiplier;

        moveDirection *= currentSpeed;
    }

    private void Move()
    {
        // Scale force application based on moveSpeed
        rb.AddForce(moveDirection * moveSpeed * movementForceMultiplier, ForceMode.Force);
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            // Gravity scales with moveSpeed to maintain balance
            // float extraGravity = IsTouchingWall() ? gravityMultiplier * 2f : gravityMultiplier;
            rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
        }
    }

    private bool IsTouchingWall()
        {
        float wallCheckDistance = 0.6f; // Distance to detect walls
        Vector3 position = transform.position;

        // ✅ Check walls in multiple directions
        bool rightWall = Physics.Raycast(position, transform.right, wallCheckDistance);
        bool leftWall = Physics.Raycast(position, -transform.right, wallCheckDistance);
        bool forwardWall = Physics.Raycast(position, transform.forward, wallCheckDistance);
        bool backwardWall = Physics.Raycast(position, -transform.forward, wallCheckDistance);

        return rightWall || leftWall || forwardWall || backwardWall;
    }
    private void CheckGround()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.4f, Vector3.down, out hit, 1.1f))
        {
            isGrounded = true;
            lastGroundedTime = Time.time;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || Time.time - lastGroundedTime < coyoteTime))
        {
            // Reset velocity before jumping to prevent stacking momentum
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * moveSpeed * jumpMultiplier, ForceMode.Impulse);
        }
    }
}
