using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health;
    private int maxHealth;
    private bool isDead;

    private CapsuleCollider col;
    private Animator anim;

    private EnemyMelee enemyMelee;
    private NavMeshAgent agent;

    [SerializeField] private float iFrameTime;
    private bool isInIframe;

    [SerializeField] private bool canBeJuggled;
    [SerializeField] private bool canBeKnockedBack;

    public bool isBlocking;
    [SerializeField] private float blockChance;
    [SerializeField] private float blockStunDuration;

    [SerializeField] private GameObject bloodBig;
    [SerializeField] private GameObject bloodSmall;

    [SerializeField] private GameObject sparkBig;
    [SerializeField] private GameObject sparkSmall;
    [SerializeField] private GameObject splat;
    [SerializeField] private Transform swordPos;
    [SerializeField] private Transform groundCheck;

    private void Awake()
    {
        maxHealth = health;
        enemyMelee = GetComponent<EnemyMelee>();
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (health <= 0 && !isDead)
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
                if (hitBox.isGuardBreak)
                {
                    blockStunDuration = 0;
                    anim.SetTrigger("BlockBreak");
                    anim.ResetTrigger("Attack1");
                    anim.ResetTrigger("Attack2");
                    anim.ResetTrigger("Attack3");
                    Instantiate(sparkBig, swordPos.position, Quaternion.identity);
                    enemyMelee.recoveryTime = 1f;
                }
                else
                {
                    //Block
                    anim.SetTrigger("Block");
                    Instantiate(sparkSmall, swordPos.position, Quaternion.identity);
                    blockStunDuration = 1f;
                }
            }
            else
            {
                health -= hitBox.damage;
                Instantiate(bloodSmall, transform.position, Quaternion.identity);
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

    private IEnumerator DeathFallTime()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(splat, groundCheck.position, splat.transform.rotation);
    }

    private void Die()
    {
        isDead = true;
        anim.SetTrigger("Death");
        this.enabled = false;
        enemyMelee.enabled = false;
        col.enabled = false;
        agent.enabled = false;
        WaveSystem.Instance.enemiesKilled++;
        WaveSystem.Instance.totalEnemiesKilled++;
        Instantiate(bloodBig, transform.position, Quaternion.identity);
        StartCoroutine(DeathFallTime());
        Destroy(gameObject, 60f);
    }
}