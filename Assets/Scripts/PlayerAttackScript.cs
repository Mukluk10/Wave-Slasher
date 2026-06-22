using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttackScript : MonoBehaviour
{
    public float recoveryTime;
    [SerializeField] private float attackCancelTime;

    private int attackCount;

    public bool isInRecovery;
    private bool canCombo;

    private CharacterController controller;
    private PlayerMovement movementScript;
    private PlayerBlock blockScript;
    private Animator anim;

    [SerializeField] private float momentumSpeed;
    [SerializeField] private float momentumDistance;
    private float maxMomentumSpeed;
    private Vector3 momentumDirection;

    public void OnAttack (InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (!blockScript.isBlocking)
            {
                if (movementScript.isGrounded)
                    BasicAttack();
                else
                    AirAttack();
            }
            else
            {
                //Counter???
            }
        }
    }
    private void Awake()
    {
        movementScript = GetComponent<PlayerMovement>();
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        blockScript = GetComponent<PlayerBlock>();
        maxMomentumSpeed = momentumSpeed;
    }

    private void Update()
    {
        anim.SetBool("Recovery", isInRecovery);
    }

    private void BasicAttack()
    {
        if(attackCount == 0 && !isInRecovery)
        {
            anim.SetTrigger("Attack");
            canCombo = false;
            attackCount++;
            ApplyMomentum();
            recoveryTime = 0.3f;
            attackCancelTime = 0.15f;
        }

        if(attackCount == 1 && canCombo)
        {
            anim.SetTrigger("Attack");
            canCombo = false;
            attackCount++;
            ApplyMomentum();
            recoveryTime = 0.3f;
            attackCancelTime = 0.15f;
        }

        if (attackCount == 2 && canCombo)
        {
            anim.SetTrigger("Attack");
            canCombo = false;
            attackCount++;
            ApplyMomentum();
            recoveryTime = 0.3f;
            attackCancelTime = 0.15f;
        }

        if (attackCount == 3 && canCombo)
        {
            anim.SetTrigger("Attack");
            canCombo = false;
            attackCount++;
            ApplyMomentum();
            recoveryTime = 0.5f;
            attackCancelTime = 0.5f;
        }


        if (movementScript.moveDirection.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(movementScript.moveDirection);
        }
    }

    private void AirAttack()
    {
        if (attackCount == 0 && !isInRecovery)
        {
            anim.SetTrigger("Attack");
            movementScript.verticalVelocity = 5;
            canCombo = false;
            attackCount++;
            recoveryTime = 0.3f;
            attackCancelTime = 0.15f;
        }

        if (attackCount == 1 && canCombo)
        {
            anim.SetTrigger("Attack");
            movementScript.verticalVelocity = 5;
            canCombo = false;
            attackCount++;
            recoveryTime = 0.3f;
            attackCancelTime = 0.3f;
        }

        if (movementScript.moveDirection.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(movementScript.moveDirection);
        }
    }

    private void FixedUpdate()
    {
        if (recoveryTime > 0)
        {
            recoveryTime -=1 * Time.fixedDeltaTime;
            isInRecovery = true;
        }
        else
        {
            isInRecovery = false;
            attackCount = 0;
        }

        if(attackCancelTime > 0)
        {
            attackCancelTime -= 1 * Time.fixedDeltaTime;
            canCombo = false;
        }
        else
        {
            canCombo = true;
        }
    }

    private IEnumerator AttackMomentum()
    {
        float dashTime = momentumDistance / momentumSpeed;
        float elapsed = 0f;

        while (elapsed < dashTime)
        {
            controller.Move(momentumDirection * momentumSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            momentumSpeed -= 65f * Time.deltaTime;

            yield return null;
        }
        momentumSpeed = maxMomentumSpeed;
    }

    private void ApplyMomentum()
    {
        momentumDirection = movementScript.moveDirection.sqrMagnitude > 0.01f ? movementScript.moveDirection.normalized : transform.forward;
        StartCoroutine(AttackMomentum());
    }
}
