using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 2.0f;
    public float gravity = -9.81f;
    public float jumpPower = 5.0f;

    public LayerMask groundMask;

    private Vector2 input;
    private Vector3 velocity;

    private bool onGround = true;
    private bool isWalking = false;
    private bool isProstrated = false;
    private bool isAttacking = false;
    private bool onLadder = false;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator animator;
    public GameManager gm;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"));

        if (onLadder)
        {
            Ladder();
            if (Input.GetKey(KeyCode.LeftAlt) && input.x != 0)
            {
                LadderJump();
            }
        }
        else
        {
            OutOfLadder();
        }
        if (rb.velocity.y < -0.2f)
            onGround = false;
        if (onGround)
        {
            if (!isProstrated && !isAttacking)
                Walking(input);

            if (!isProstrated && !isAttacking
                && Input.GetKey(KeyCode.LeftAlt))
                NormalJump();

            if (input.y < 0)
            {
                isProstrated = true;
                velocity.x = 0;
            }
            else
                isProstrated = false;

            if (isAttacking)
            {
                velocity.x = 0;
            }
        }
        else
        {
            velocity.y += gravity * Time.fixedDeltaTime;
        }

        if (!isAttacking && !isProstrated 
            && Input.GetKey(KeyCode.LeftControl))
        {
            StartCoroutine(NormalAttack());
        }

        if (velocity.x > 0)
            sr.flipX = true;
        else if (velocity.x < 0)
            sr.flipX = false;

        transform.Translate(velocity * Time.fixedDeltaTime);

        SetAnimatorBool();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || 
            col.gameObject.CompareTag("Platform") && velocity.y <= 2.0f)
        {
            onGround = true;
            velocity.y = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Ladder") && input.y > 0
            || col.CompareTag("LadderTop") && input.y < 0)
        {
            transform.position = new Vector3(
                col.transform.position.x,
                transform.position.y,
                transform.position.z);
            onLadder = true;
        }

        if (col.CompareTag("Portal") && input.y > 0)
        {
            gm.MapChange();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ladder") && input.y <= 0
            || col.CompareTag("LadderTop") && input.y >= 0)
        {
            onLadder = false;
        }
    }

    private void Walking(Vector2 input)
    {
        velocity.x = input.x * moveSpeed;
        isWalking = true;
        
        if (velocity.x == 0)
            isWalking = false;
    }

    private void NormalJump()
    {
        onGround = false;
        velocity.y = jumpPower;
    }

    private void Ladder()
    {
        onGround = false;
        isWalking = false;
        this.gameObject.layer = 11;
        gravity = 0;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(0, 0);
        velocity.x = 0;
        velocity.y = input.y * moveSpeed;
        animator.speed = Mathf.Abs(velocity.y);
    }

    private void LadderJump()
    {
        onLadder = false;
        OutOfLadder();
        velocity.x = input.x * 2;
        velocity.y = 2;
    }

    private void OutOfLadder()
    {
        gravity = -9.81f;
        rb.gravityScale = 1;
        this.gameObject.layer = 9;
        animator.speed = 1;
    }

    IEnumerator NormalAttack()
    {
        isAttacking = true;
        animator.SetTrigger("doAttack");
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
    }

    private void SetAnimatorBool()
    {
        animator.SetBool("onGround", onGround);
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isProstrated", isProstrated);
        animator.SetBool("onLadder", onLadder);
    }
}
