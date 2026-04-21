using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private float originalSpeed;
    private float originalJumpForce;
    private bool isSpeedBoost = false;

    public Transform groundCheck;
    public LayerMask groundLayer;

    public GameObject carrotBulletPrefab;
    public Transform firePoint;

    private bool canAttack = false;

    private Rigidbody2D rb;
    private Animator pAni;
    private SpriteRenderer sr;
    private bool isGrounded;


    private bool isGiant = false;

    private float moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        originalSpeed = moveSpeed;
        originalJumpForce = jumpForce;
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);


        if (isGiant)
        {
            if (moveInput < 0)
                transform.localScale = new Vector3(2, 2, 2);
            else if (moveInput > 0)
                transform.localScale = new Vector3(-2, 2, 2);
        }
        else
        {
            if(moveInput < 0)
                transform.localScale = new Vector3 (1, 1, 1);
            else if(moveInput > 0)
                transform.localScale = new Vector3 (-1, 1, 1);
        }

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);


            isGrounded = Physics2D.OverlapCircle(groundCheck.position, .2f, groundLayer);

        if (canAttack && Keyboard.current.zKey.wasPressedThisFrame)
        {
            Shoot();
        }
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(carrotBulletPrefab, firePoint.position, Quaternion.identity);

        float dir = transform.localScale.x > 0 ? 1f : -1f;

        bullet.GetComponent<CarrotBullet>().SetDirection(dir);
    }

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveInput = input.x;
    }

    public void OnJump(InputValue value)
    {
        if(value.isPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            pAni.SetTrigger("Jump");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.CompareTag("Finish"))
        {
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        if (collision.CompareTag("Enemy"))
        {
            if (isGiant)
                Destroy(collision.gameObject);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (collision.CompareTag("Item"))
        {
            isGiant = true;

            sr.color = new Color(0.8f, 0.4f, 1f);

            CancelInvoke(nameof(ResetGiant));
            Invoke(nameof(ResetGiant), 3f);

            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("SpeedItem"))
        {
            SpeedBoost();
            StartCoroutine(YellowBlink());
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("JumpItem"))
        {
            JumpBoost();
            StartCoroutine(JumpGlow());
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("AttackItem"))
        {
            canAttack = true;
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Boss"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void ResetGiant()
    {
        isGiant = false;
        sr.color = Color.white;
    }
    void SpeedBoost()
    {
        moveSpeed = 8f;

        CancelInvoke(nameof(ResetSpeed));
        Invoke(nameof(ResetSpeed), 3f);
    }

    void ResetSpeed()
    {
        moveSpeed = originalSpeed;
    }

    IEnumerator YellowBlink()
    {
        for (int i = 0; i < 13; i++)
        {
            sr.color = new Color(1f, 0.85f, 0f);
            yield return new WaitForSeconds(0.1f);

            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        sr.color = Color.white;
    }
    void JumpBoost()
    {
        jumpForce = originalJumpForce + 3f;

        CancelInvoke(nameof(ResetJump));
        Invoke(nameof(ResetJump), 3f);
    }

    void ResetJump()
    {
        jumpForce = originalJumpForce;
    }
    IEnumerator JumpGlow()
    {
        for (int i = 0; i < 13; i++)
        {
            sr.color = new Color(0.3f, 0.6f, 1f);
            yield return new WaitForSeconds(0.1f);

            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        sr.color = Color.white;
    }
}
