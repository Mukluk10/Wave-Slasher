using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] private float moveSpeed;


    [SerializeField] private bool inReadyDistance;
    [SerializeField] private bool readyToAttack;
    private bool attacking;

    private NavMeshAgent agent;

    [SerializeField] private GameObject player;
    [SerializeField] private float readyDistance;
    [SerializeField] private float attackRange;
    [SerializeField] private float distance;

    [SerializeField] private bool canBeJuggled;
    [SerializeField] private bool canBeKnockedBack;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent.speed = moveSpeed;
    }

    private void FixedUpdate()
    {
        if(player != null)
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

        if(distance < attackRange && readyToAttack)
        {
            Attack();
        }
        
    }

    private void Update()
    {
        if (inReadyDistance && !attacking)
        {
            Vector3 relativePos = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;
        }
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
        //Attack
    }

    private IEnumerator AttackTimeDivergence()
    {
        float randomAttackTime = Random.Range(0.5f, 2f);
        yield return new WaitForSeconds(randomAttackTime);
        GoingToAttack();
    }
}
