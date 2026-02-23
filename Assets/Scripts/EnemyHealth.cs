using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health;
    private int maxHealth;

    private EnemyMelee enemyMelee;

    private float iFrameTime = 0.1f;
    private bool isInIframe;

    private void Awake()
    {
        maxHealth = health;
        enemyMelee = GetComponent<EnemyMelee>();
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