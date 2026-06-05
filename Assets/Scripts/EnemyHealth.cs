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

    private void Awake()
    {
        maxHealth = health;
        enemyMelee = GetComponent<EnemyMelee>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (health <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack") && !isInIframe)
        {
            Debug.Log("Enemy Got Hit");
            Hitbox hitBox = other.GetComponent<Hitbox>();
            health -= hitBox.damage;
            Debug.Log("Took Damage: " + hitBox.damage);
            enemyMelee.recoveryTime = 0.5f;
            anim.SetTrigger("Damage");
            isInIframe = true;
            StartCoroutine(Invinciblity());
        }
    }

    private IEnumerator Invinciblity()
    {
        yield return new WaitForSeconds(iFrameTime);
        isInIframe = false;
    }
}