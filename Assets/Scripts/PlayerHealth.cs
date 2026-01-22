using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health;
    private int maxHealth;

    private PlayerMovement movementScript;
    private PlayerAttackScript attakScript;

    private float iFrameTime;

    private void Awake()
    {
        maxHealth = health;
        movementScript = GetComponent<PlayerMovement>();
        attakScript = GetComponent<PlayerAttackScript>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            EnemyHitbox hitBox = other.GetComponent<EnemyHitbox>();
            health -= hitBox.damage;
            Debug.Log("Took Damage: " + hitBox.damage);
        }
    }
}
