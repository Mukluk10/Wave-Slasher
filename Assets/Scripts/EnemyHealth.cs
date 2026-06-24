using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health;
    private int maxHealth;
    private Animator anim;

    private EnemyMelee enemyMelee;

    [SerializeField] private float iFrameTime;
    private bool isInIframe;

    [SerializeField] private bool canBeJuggled;
    [SerializeField] private bool canBeKnockedBack;

    public bool isBlocking;
    [SerializeField] private float blockChance;
    [SerializeField] private float blockStunDuration;


    private void Awake()
    {
        maxHealth = health;
        enemyMelee = GetComponent<EnemyMelee>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (health <= 0)
            Die();
    }

    private void FixedUpdate()
    {
        if (blockStunDuration > 0)
        {
            blockStunDuration -= 1 * Time.fixedDeltaTime;
            isBlocking = true;
        }
        else
        {
            isBlocking = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack") && !isInIframe)
        {
            Hitbox hitBox = other.GetComponent<Hitbox>();

            float blockRoll = Random.Range(0, 100);
            if ((blockRoll <= blockChance || isBlocking) && !enemyMelee.isInRecovery && !enemyMelee.isAirStunned && !enemyMelee.isOTG)
            {
                //Block
                anim.SetTrigger("Block");
                blockStunDuration = 0.33f;
            }
            else
            {
                //Debug.Log("Enemy Got Hit");

                health -= hitBox.damage;
                //Debug.Log("Took Damage: " + hitBox.damage);
                if (hitBox.isKnockback)
                {
                    enemyMelee.KnockDown();
                }
                else if (hitBox.isJuggle)
                {
                    enemyMelee.AirKnockBack();
                    anim.SetTrigger("Juggle");
                }
                else
                {
                    if (enemyMelee.isOTG)
                    {
                        enemyMelee.KnockDown();
                    }
                    else if (enemyMelee.isAirStunned)
                    {
                        enemyMelee.AirKnockBack();
                        anim.SetTrigger("Juggle");
                    }
                    else
                    {
                        enemyMelee.recoveryTime = 0.5f;
                        anim.SetTrigger("Damage");
                    }
                }
                

                isInIframe = true;
                StartCoroutine(Invinciblity());
            }
        }
    }

    private IEnumerator Invinciblity()
    {
        yield return new WaitForSeconds(iFrameTime);
        isInIframe = false;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}