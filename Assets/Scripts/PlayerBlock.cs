using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlock : MonoBehaviour
{
    private PlayerMovement movementScript;
    private PlayerAttackScript attackScript;

    private bool canGuardBuffer;
    public bool isBlocking;

    private Animator anim;
    public void OnBlock(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (movementScript.isGrounded)
                Block();
        }
        if (ctx.canceled)
        {
            if (movementScript.isGrounded)
                StopBlocking();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GetComponent<PlayerMovement>();
        attackScript = GetComponent<PlayerAttackScript>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Blocking", isBlocking);
    }

    private void Block()
    {
        if (!attackScript.isInRecovery)
        {
            isBlocking = true;
        }
    }

    public void StopBlocking()
    {
        isBlocking = false;
    }
}
