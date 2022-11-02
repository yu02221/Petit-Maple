using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float gravity = -9.81f;
    public float jumpPower = 5.0f;

    public LayerMask groundMask;

    private Vector3 velocity;

    private bool onGround = true;
    private bool isWalking = false;
    private bool isProstrated = false;
    private bool isAttacking = false;

    public SpriteRenderer sr;

    public Animator player;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"));

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

        transform.Translate(velocity * Time.fixedDeltaTime);

        SetAnimatorBool();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
        if (col.gameObject.CompareTag("Platform")
            && velocity.y <= 1.0f)
        {
            onGround = true;
        }
    }
    /*
    private void CheckOnGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundMask);
        print(hit.distance);
        if (hit.distance < 0.1f && velocity.y <= 2.0f)
        {
            onGround = true;
            velocity.y = 0;
        }
        else
        {
            onGround = false;
        }
    }
    */

    private void Walking(Vector2 input)
    {
        velocity.x = input.x * moveSpeed;
        isWalking = true;
        if (velocity.x > 0)
            sr.flipX = true;
        else if (velocity.x < 0)
            sr.flipX = false;
        else
            isWalking = false;
    }

    private void NormalJump()
    {
        onGround = false;
        velocity.y = jumpPower;
    }

    IEnumerator NormalAttack()
    {
        isAttacking = true;
        player.SetTrigger("doAttack");
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
    }

    private void SetAnimatorBool()
    {
        player.SetBool("onGround", onGround);
        player.SetBool("isWalking", isWalking);
        player.SetBool("isProstrated", isProstrated);
    }
}
