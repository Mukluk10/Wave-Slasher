using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpHeight;

    private CharacterController controller;

    [SerializeField] private Transform cam;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;

    private Vector2 moveInput;
    [SerializeField] private bool isGrounded;
    private bool canEnemyStep;
    private bool isWalking;

    

    private PlayerAttackScript attakScript;

    public Vector3 moveDirection;
    public float verticalVelocity;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if(isGrounded)
            Jump();
        }
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        attakScript = GetComponent<PlayerAttackScript>();
    }

    // Update is called once per frame
    private void Update()
    {
        Movement();
        if (moveInput.sqrMagnitude < 0.01f ? isWalking = false : isWalking = true) ;

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = new Vector3(0f, verticalVelocity, 0f);
        controller.Move(velocity * Time.deltaTime);
    }

    private void Movement()
    {
        if (moveInput.sqrMagnitude < 0.01f)
            return;

        // Get camera-relative directions
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        // Calculate movement direction
        moveDirection = camForward * moveInput.y + camRight * moveInput.x;

        if (attakScript.isInRecovery)
            return;

        // Move the character
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Rotate towards movement direction
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        Debug.Log("Jumped!");
        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}
