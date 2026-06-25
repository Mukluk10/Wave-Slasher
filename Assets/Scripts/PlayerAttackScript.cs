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

    [SerializeField] private LayerMask enemyLayer;
    public Transform currentTarget;
    private Vector3 attackDir;

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

    public void OnSpecial (InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (!blockScript.isBlocking)
            {
                if (movementScript.isGrounded)
                    SpecialAttack();
                else
                    AirSpecial();
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
        FindClosestEnemy();

        if (movementScript.moveDirection.sqrMagnitude > 0.5f)
        {
            transform.rotation = Quaternion.LookRotation(movementScript.moveDirection);
        }
        else
        {
            Debug.Log("AutoLockOn");
            if (currentTarget != null)
            {
                attackDir = currentTarget.position - transform.position;
                attackDir.y = 0f;
                transform.rotation = Quaternion.LookRotation(attackDir);
            }
        }

        if (attackCount == 0 && !isInRecovery)
        {
            anim.SetTrigger("Attack");
            canCombo = false;
            attackCount++;
            recoveryTime = 0.3f;
            attackCancelTime = 0.15f;
        }

        if(attackCount == 1 && canCombo)
        {
            anim.SetTrigger("Attack");
            canCombo = false;
            attackCount++;
            recoveryTime = 0.3f;
            attackCancelTime = 0.15f;
        }

        if (attackCount == 2 && canCombo)
        {
            anim.SetTrigger("Attack");
            canCombo = false;
            attackCount++;
            recoveryTime = 0.3f;
            attackCancelTime = 0.15f;
        }

        if (attackCount == 3 && canCombo)
        {
            anim.SetTrigger("Attack");
            canCombo = false;
            attackCount++;
            recoveryTime = 0.5f;
            attackCancelTime = 0.5f;
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

    private void SpecialAttack()
    {
        FindClosestEnemy();

        if (movementScript.moveDirection.sqrMagnitude > 0.5f)
        {
            transform.rotation = Quaternion.LookRotation(movementScript.moveDirection);
        }
        else
        {
            Debug.Log("AutoLockOn");
            if (currentTarget != null)
            {
                attackDir = currentTarget.position - transform.position;
                attackDir.y = 0f;
                transform.rotation = Quaternion.LookRotation(attackDir);
            }
        }

        if (attackCount == 0 && canCombo)
        {
            anim.SetTrigger("Special");
            canCombo = false;
            recoveryTime = 1.4f;
            attackCancelTime = 1.4f;
        }

        if (attackCount == 1 && canCombo)
        {
            anim.SetTrigger("Special");
            canCombo = false;
            recoveryTime = 0.4f;
            attackCancelTime = 0.4f;
        }

        if (attackCount == 2 && canCombo)
        {
            anim.SetTrigger("Special");
            canCombo = false;
            recoveryTime = 0.6f;
            attackCancelTime = 0.6f;
        }
    }

    private void AirSpecial()
    {

    }

    public void FindClosestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 2.5f, enemyLayer);

        currentTarget = null;
        float closestDistSqr = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            float distSqr = (hit.transform.position - transform.position).sqrMagnitude;
            if (distSqr < closestDistSqr)
            {
                closestDistSqr = distSqr;
                currentTarget = hit.transform;
            }
        }
        Debug.Log($"Hits found: {hits.Length}, target: {currentTarget}");
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

    public void ApplyMomentum()
    {
        if (movementScript.moveDirection.sqrMagnitude > 0.5f)
        {
            momentumDirection = movementScript.moveDirection.normalized;
        }
        else if (currentTarget != null)
        {
            attackDir = currentTarget.position - transform.position;
            attackDir.y = 0f;
            momentumDirection = attackDir;
        }
        else
        {
            momentumDirection = transform.forward;
        }

        StartCoroutine(AttackMomentum());
    }
}
