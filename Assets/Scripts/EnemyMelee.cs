using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] private float moveSpeed;


    [SerializeField] private bool inReadyDistance;
    [SerializeField] private bool readyToAttack;
    public float recoveryTime;
    private bool isInRecovery;
    private bool attacking;
    private bool isMoving;

    public bool isOTG;
    public bool isAirStunned;

    private NavMeshAgent agent;

    [SerializeField] private GameObject player;
    [SerializeField] private float readyDistance;
    [SerializeField] private float attackRange;
    [SerializeField] private float distance;

    [SerializeField] private int attackCount;
    [SerializeField] private float knockBackTime;

    [SerializeField] private Animator anim;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;
    private Rigidbody rb;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent.speed = moveSpeed;
    }

    private void FixedUpdate()
    {
        if(player != null && !isInRecovery && !isOTG && !isAirStunned)
        {
            agent.SetDestination(player.transform.position);
        }

        distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        if(distance < readyDistance && !inReadyDistance && !isOTG)
        {
            IsReadyDistance();
        }

        if(distance > readyDistance && inReadyDistance)
        {
            LostReadyDistance();
        }

        if(distance < attackRange && readyToAttack && !isInRecovery)
        {
            Attack();
        }

        if (recoveryTime > 0)
        {
            recoveryTime -= 1 * Time.fixedDeltaTime;
            isInRecovery = true;
        }
        else
        {
            isInRecovery = false;
        }
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        if (inReadyDistance && !attacking)
        {
            Vector3 relativePos = player.transform.position - transform.position;
            relativePos.y = 0f;
            if (isOTG || isAirStunned)
                return;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;
        }

        if(isGrounded && isAirStunned)
        {
            KnockDown();
        }

        if (agent.speed > 0.01f)
            isMoving = true;
        else
            isMoving = false;

        anim.SetBool("Moving", isMoving);
    }
    private void IsReadyDistance()
    {
        readyDistance += 1f;
        inReadyDistance = true;
        agent.speed = 0;
        StartCoroutine(AttackTimeDivergence());
        //When to attack
    }

    private void LostReadyDistance()
    {
        readyDistance -= 1f;
        agent.speed = moveSpeed;
        StopAllCoroutines();
        readyToAttack = false;
        inReadyDistance = false;
        //move towards player
    }

    private void GoingToAttack()
    {
        agent.speed = moveSpeed;
        readyToAttack = true;
    }

    private void Attack()
    {
        //Random attack somehow
        Attack1();
    }

    private IEnumerator AttackTimeDivergence()
    {
        float randomAttackTime = Random.Range(0.5f, 2f);
        yield return new WaitForSeconds(randomAttackTime);
        GoingToAttack();
    }

    private void Attack1()
    {
        anim.SetTrigger("Attack");
        recoveryTime = 1f;
    }

    public void AirKnockBack()
    {
        if (isGrounded)
        {
            isOTG = false;
            agent.enabled = false;
            rb.isKinematic = false;
            rb.AddForce(transform.up * 5f, ForceMode.Impulse);
            isAirStunned = true;
            recoveryTime = 99999;
            StopAllCoroutines();
        }
        else
        {
            rb.AddForce(transform.up * 5f, ForceMode.Impulse);
        }
    }

    public void KnockDown()
    {
        agent.enabled = true;
        rb.isKinematic = true;
        isAirStunned = false;
        isOTG = true;
        recoveryTime = knockBackTime;
        anim.SetTrigger("KnockDown");
        StopAllCoroutines();
        StartCoroutine(KnockDownState());
    }

    private IEnumerator KnockDownState()
    {
        yield return new WaitForSeconds(knockBackTime);
        isOTG = false;
    }
}
