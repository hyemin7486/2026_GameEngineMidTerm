using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("HP")]
    public int hp = 3;
    public GameObject hp3;
    public GameObject hp2;
    public GameObject hp1;

    [Header("Target")]
    public Transform player;

    [Header("Move")]
    public float moveSpeed = 2f;
    public float dashSpeed = 7f;
    public float jumpForce = 8f;

    [Header("Attack Timing")]
    public float attackInterval = 2f;

    private Rigidbody2D rb;
    private bool isDead = false;
    private bool isAttacking = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        UpdateHPUI();
        StartCoroutine(BossPattern());
    }

    void Update()
    {
        if (isDead || isAttacking || player == null) return;

        float dir = player.position.x > transform.position.x ? 1f : -1f;

        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        if (dir > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    IEnumerator BossPattern()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(attackInterval);

            if (player == null) continue;

            int pattern = Random.Range(0, 2);

            if (pattern == 0)
                yield return StartCoroutine(JumpAttack());
            else
                yield return StartCoroutine(DashAttack());
        }
    }

    IEnumerator JumpAttack()
    {
        isAttacking = true;

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        yield return new WaitForSeconds(0.3f);

        float dir = player.position.x > transform.position.x ? 1f : -1f;
        rb.linearVelocity = new Vector2(dir * 3f, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1.2f);

        isAttacking = false;
    }

    IEnumerator DashAttack()
    {
        isAttacking = true;

        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.4f);

        float dir = player.position.x > transform.position.x ? 1f : -1f;
        rb.linearVelocity = new Vector2(dir * dashSpeed, rb.linearVelocity.y);

        yield return new WaitForSeconds(0.8f);

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        isAttacking = false;
    }

    public void TakeDamage()
    {
        if (isDead) return;

        hp--;
        UpdateHPUI();

        if (hp <= 0)
        {
            Die();
        }
    }

    void UpdateHPUI()
    {
        if (hp3 != null) hp3.SetActive(hp >= 3);
        if (hp2 != null) hp2.SetActive(hp >= 2);
        if (hp1 != null) hp1.SetActive(hp >= 1);
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(DieRoutine());
    }

    IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Ending");
    }
}