using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health;
    private int maxHealth;

    private PlayerMovement movementScript;
    private PlayerAttackScript attakScript;

    private float iFrameTime = 0.1f;
    private bool isInIframe;

    private void Awake()
    {
        maxHealth = health;
        movementScript = GetComponent<PlayerMovement>();
        attakScript = GetComponent<PlayerAttackScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack") && !isInIframe)
        {
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
