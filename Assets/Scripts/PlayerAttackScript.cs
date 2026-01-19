using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackScript : MonoBehaviour
{
    [SerializeField] private float recoveryTime;
    [SerializeField] private float attackCancelTime;

    private int attackCount;

    public bool isInRecovery;
    private bool canCombo;

    private PlayerMovement movementScript;

    public void OnAttack (InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            BasicAttack();
        }
    }
    private void Awake()
    {
        movementScript = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BasicAttack()
    {
        if(attackCount == 0 && !isInRecovery)
        {
            // 1st attack
            // anim play
            canCombo = false;
            recoveryTime = 0.3f;
            attackCancelTime = 0.15f;
            Debug.Log("1st attack!");
            attackCount++;
        }

        if(attackCount == 1 && canCombo)
        {
            //2nd attack
            canCombo = false;
            recoveryTime = 0.5f;
            attackCancelTime = 0.5f;
            Debug.Log("2nd attack!");
            attackCount++;
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
}
