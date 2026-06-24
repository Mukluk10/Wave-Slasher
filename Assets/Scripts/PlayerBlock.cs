using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlock : MonoBehaviour
{
    private PlayerMovement movementScript;
    private PlayerAttackScript attackScript;

    private bool canGuardBuffer;
    private bool isPressingBlock;
    private bool isInBlockRecovery;
    public bool isBlocking;

    private Animator anim;
    public void OnBlock(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isPressingBlock = true;
        }
        if (ctx.canceled)
        {
            isPressingBlock = false;
        }
    }

    private void Awake()
    {
        movementScript = GetComponent<PlayerMovement>();
        attackScript = GetComponent<PlayerAttackScript>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!attackScript.isInRecovery && isPressingBlock && movementScript.isGrounded && !isBlocking && !isInBlockRecovery && !movementScript.isDashing)
        {
            Block();
        }

        if(isBlocking && !isPressingBlock)
        {
            StopBlocking();
        }

        anim.SetBool("Blocking", isBlocking);
    }

    private void Block()
    {
        Debug.Log("blocking");
        isBlocking = true;
    }

    public void StopBlocking()
    {
        Debug.Log("stop blocking");
        isInBlockRecovery = true;
        isBlocking = false;
        StartCoroutine(BlockRecovery());
    }

    public void GuardBreak()
    {
        anim.SetTrigger("BlockBreak");
        isBlocking = false;
        attackScript.recoveryTime = 0.8f;
        StartCoroutine(BlockRecovery());
    }

    private IEnumerator BlockRecovery()
    {
        yield return new WaitForSeconds(0.2f);
        isInBlockRecovery = false;
    }
}
