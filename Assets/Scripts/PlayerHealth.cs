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
            if (!blockScript.isBlocking)
            {
                Hitbox hitBox = other.GetComponent<Hitbox>();
                health -= hitBox.damage;
                Debug.Log("Took Damage: " + hitBox.damage);
                anim.SetTrigger("Damage");
                attakScript.recoveryTime = 0.2f;
                isInIframe = true;
                StartCoroutine(Invinciblity());
            }
            else
            {
                //Blocked attack;
            }
        }
    }

    private IEnumerator Invinciblity()
    {
        yield return new WaitForSeconds(iFrameTime);
        isInIframe = false;
    }
}
