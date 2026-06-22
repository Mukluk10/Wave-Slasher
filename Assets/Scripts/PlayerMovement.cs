using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float enemyStepHeight;

    private float maxDashSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDistance;

    private CharacterController controller;
    private PlayerAttackScript attakScript;
    private PlayerBlock blockScript;

    [SerializeField] private Transform cam;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Animator anim;

    private Vector2 moveInput;
    private Vector3 dashDirection;
    private bool canEnemyStep;
    private bool isWalking;

    

    public bool isGrounded;
    public Vector3 moveDirection;
    public float verticalVelocity;
    public bool isDashing;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (isGrounded && !attakScript.isInRecovery && !isDashing)
            {
                dashDirection = moveDirection.sqrMagnitude > 0.01f ? moveDirection.normalized : transform.forward;
                StartCoroutine(Dash());
            }
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if(isGrounded && !attakScript.isInRecovery)
            Jump();

            if (canEnemyStep)
                EnemyStep();
        }
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        attakScript = GetComponent<PlayerAttackScript>();
        blockScript = GetComponent<PlayerBlock>();
        maxDashSpeed = dashSpeed;
    }

    private void Update()
    {
        Movement();
        if (moveInput.sqrMagnitude < 0.01f ? isWalking = false : isWalking = true) ;

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        anim.SetBool("Moving", isWalking);
        anim.SetBool("Grounded", isGrounded);
        anim.SetBool("Dodging", isDashing);

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

        if (attakScript.isInRecovery || isDashing || blockScript.isBlocking)
            return;

        // Move the character
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Rotate towards movement direction
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        anim.SetTrigger("Jump");
        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        blockScript.StopBlocking();
    }

    private void EnemyStep()
    {
        Debug.Log("EnemyStep");
        verticalVelocity = Mathf.Sqrt(enemyStepHeight * -2f * gravity);
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        attakScript.recoveryTime = 0.5f;
        blockScript.StopBlocking();

        float dashTime = dashDistance / dashSpeed;
        float elapsed = 0f;

        while(elapsed < dashTime)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            dashSpeed -= 65f * Time.deltaTime;

            yield return null;
        }

        isDashing = false;
        dashSpeed = maxDashSpeed;
    }
}
