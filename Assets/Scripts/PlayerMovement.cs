using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    [SerializeField] private float rotationSpeed = 180f;

    private CharacterController controller;

    [SerializeField] private Transform cam;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;

    private Vector2 moveInput;
    private bool isGrounded;
    private bool canEnemyStep;
    

    private PlayerAttackScript attakScript;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isGrounded)
        {
            //JUMP
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
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
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
        Vector3 moveDirection = camForward * moveInput.y + camRight * moveInput.x;

        if (attakScript.isInRecovery)
            return;

        // Move the character
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Rotate towards movement direction
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
