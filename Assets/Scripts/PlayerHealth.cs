using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private bool isDead;

    [SerializeField] private GameObject bloodBig;
    [SerializeField] private GameObject bloodSmall;

    [SerializeField] private GameObject sparkSmall;
    [SerializeField] private Transform swordPos;

    [SerializeField] private Slider hPBar;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private string sceneToLoadOnDeath;

    private PlayerInput input;

    private void Awake()
    {
        maxHealth = health;
        movementScript = GetComponent<PlayerMovement>();
        attakScript = GetComponent<PlayerAttackScript>();
        blockScript = GetComponent<PlayerBlock>();
        anim = GetComponentInChildren<Animator>();
        input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (health <= 0 && !isDead)
            Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack") && !isInIframe && !isDead)
        {
            Hitbox hitBox = other.GetComponent<Hitbox>();

            if (!blockScript.isBlocking)
            {
                health -= hitBox.damage;
                Instantiate(bloodSmall, transform.position, Quaternion.identity);
                anim.SetTrigger("Damage");
                attakScript.recoveryTime = 0.2f;
                hPBar.value = health;
                isInIframe = true;
                StartCoroutine(Invinciblity());
            }
            else
            {
                if (hitBox.isGuardBreak)
                {
                    blockScript.GuardBreak();
                }
                else
                {
                    anim.SetTrigger("BlockStun");
                    Instantiate(sparkSmall, swordPos.position, Quaternion.identity);
                    attakScript.recoveryTime = 0.2f;

                    attakScript.FindClosestEnemy();

                    Vector3 attackDir = attakScript.currentTarget.position - transform.position;
                    attackDir.y = 0f;
                    transform.rotation = Quaternion.LookRotation(attackDir);
                }
            }
        }

        if (other.CompareTag("DeathBarrier"))
            Die();
    }

    public void Heal()
    {
        health += 25;
        hPBar.value = health;
    }

    public void Die()
    {
        isDead = true;
        anim.SetTrigger("Death");
        movementScript.enabled = false;
        attakScript.enabled = false;
        blockScript.enabled = false;
        input.enabled = false;
        deathUI.SetActive(true);
        StartCoroutine(DeathTime());
        this.enabled = false;
    }

    private IEnumerator Invinciblity()
    {
        yield return new WaitForSeconds(iFrameTime);
        isInIframe = false;
    }

    private IEnumerator DeathTime()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(sceneToLoadOnDeath);
    }
}
