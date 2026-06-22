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

    private NavMeshAgent agent;

    [SerializeField] private GameObject player;
    [SerializeField] private float readyDistance;
    [SerializeField] private float attackRange;
    [SerializeField] private float distance;

    [SerializeField] private int attackCount;

    [SerializeField] private bool canBeJuggled;
    [SerializeField] private bool canBeKnockedBack;

    [SerializeField] private Animator anim;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent.speed = moveSpeed;
    }

    private void FixedUpdate()
    {
        if(player != null && !isInRecovery)
        {
            agent.SetDestination(player.transform.position);
        }

        distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        if(distance < readyDistance && !inReadyDistance)
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
        if (inReadyDistance && !attacking)
        {
            Vector3 relativePos = player.transform.position - transform.position;
            relativePos.y = 0f;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;
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
        Debug.Log("Going to attack");
    }

    private void Attack()
    {
        Debug.Log("EnemyMeleeAttacked");
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
}
