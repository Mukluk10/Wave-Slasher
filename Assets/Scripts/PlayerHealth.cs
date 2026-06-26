using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health;
    private int maxHealth;

    private PlayerMovement movementScript;
    private PlayerAttackScript attakScript;
    private PlayerBlock blockScript;
    private Animator anim;

    private float iFrameTime = 0.1f;
    private bool isInIframe;

    [SerializeField] private GameObject bloodBig;
    [SerializeField] private GameObject bloodSmall;

    private void Awake()
    {
        maxHealth = health;
        movementScript = GetComponent<PlayerMovement>();
        attakScript = GetComponent<PlayerAttackScript>();
        blockScript = GetComponent<PlayerBlock>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack") && !isInIframe)
        {
            Hitbox hitBox = other.GetComponent<Hitbox>();

            if (!blockScript.isBlocking)
            {
                health -= hitBox.damage;
                Instantiate(bloodSmall, transform.position, Quaternion.identity);
                anim.SetTrigger("Damage");
                attakScript.recoveryTime = 0.2f;
                isInIframe = true;
                StartCoroutine(Invinciblity());
            }
            else
            {
                if (hitBox.isGuardBreak)
                {
                    blockScript.GuardBreak();
                }
                else
                {
                    anim.SetTrigger("BlockStun");
                    attakScript.recoveryTime = 0.2f;

                    attakScript.FindClosestEnemy();

                    Vector3 attackDir = attakScript.currentTarget.position - transform.position;
                    attackDir.y = 0f;
                    transform.rotation = Quaternion.LookRotation(attackDir);
                }
            }
        }
    }

    private IEnumerator Invinciblity()
    {
        yield return new WaitForSeconds(iFrameTime);
        isInIframe = false;
    }
}
