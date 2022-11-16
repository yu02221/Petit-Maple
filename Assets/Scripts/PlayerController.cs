using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 2.0f;
    public float gravity = -9.81f;
    public float jumpPower = 5.0f;
    public float upJumpPower = 15.0f;
    public float doubbleJumpPower = 10.0f;

    public LayerMask groundMask;

    private Vector2 input;
    private Vector3 velocity;

    private bool onGround = true;
    private bool isWalking = false;
    private bool isProstrated = false;
    private bool isAttacking = false;
    private bool onLadder = false;
    private bool onRope = false;
    private bool inHiddenPortal = false;
    private bool supperJump = false;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator animator;
    public LayerMask groundCheckLayerMask;

    private Transform hiddenPortalDest;

    public GameObject normalAttackSkill;
    public GameObject boltSkill;
    public GameObject boltSkill_up;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (inHiddenPortal && Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position = hiddenPortalDest.position;
        }
    }

    private void FixedUpdate()
    {
        KeyboardInput();

        GroundCheck();
        
        Move();

        SetAnimatorBool();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!isAttacking)
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
            if (col.CompareTag("Rope") && input.y > 0
                || col.CompareTag("RopeTop") && input.y < 0)
            {
                transform.position = new Vector3(
                    col.transform.position.x,
                    transform.position.y,
                    transform.position.z);
                onRope = true;
            }

            if (col.name == "LeftPortal" && input.y > 0)
            {
                GameManager.instance.GoLeftField();
            }
            if (col.name == "RightPortal" && input.y > 0)
            {
                GameManager.instance.GoRightField();
            }

            if (col.CompareTag("HiddenPortal"))
            {
                inHiddenPortal = true;
                hiddenPortalDest = col.gameObject.GetComponent<HiddenPortal>().destination;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ladder") && input.y <= 0
            || col.CompareTag("LadderTop") && input.y >= 0)
        {
            onLadder = false;
            QuitClimb();
        }
        if (col.CompareTag("Rope") && input.y <= 0
            || col.CompareTag("RopeTop") && input.y >= 0)
        {
            onRope = false;
            QuitClimb();
        }
        if (col.CompareTag("HiddenPortal"))
            inHiddenPortal = false;
    }

    private void KeyboardInput()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"));

        if (!isAttacking && !isProstrated && !onLadder && !onRope)
        {
            if (input.x > 0)
            {
                sr.flipX = true;
                Player.instance.lookDirection = -1;
            }
            else if (input.x < 0)
            {
                sr.flipX = false;
                Player.instance.lookDirection = 1;
            }
        }
    }

    private void GroundCheck()
    {
        onGround = Physics2D.OverlapCircle(
            transform.position, 0.1f, groundCheckLayerMask)
            && velocity.y <= 2.0f;

        if (onGround)
        {
            velocity.y = 0;
            supperJump = false;
        }
    }

    private void Move()
    {
        if (rb.velocity.y < -0.2f)
            onGround = false;
        // 지면에 서 있을 때만 가능
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
                if (Input.GetKey(KeyCode.LeftAlt))
                    StartCoroutine(DownJump());
            }
            else
                isProstrated = false;

            if (isAttacking)
            {
                velocity.x = 0;
            }
        }
        else
        {   // 공중에 있을 때 떨어지는 속도 증가
            if (velocity.y > -2f)
                velocity.y += gravity * Time.fixedDeltaTime;
            if (!supperJump && Input.GetKeyDown(KeyCode.LeftAlt) && !(onLadder || onRope))
            {
                Bolt();
            }
        }

        if (onLadder || onRope)
        {
            Climb();
        }

        if (Input.GetKey(KeyCode.LeftControl) &&
            !isAttacking && !isProstrated && !onLadder && !onRope)
        {
            StartCoroutine(NormalAttack());
        }
        // 플레이어 이동
        transform.Translate(velocity * Time.fixedDeltaTime);
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

    private void Bolt()
    {
        supperJump = true;
        if (input.y > 0)
        {
            Instantiate(boltSkill_up);
            velocity.y = upJumpPower;
        }
        else
        {
            Instantiate(boltSkill);
            velocity.x = doubbleJumpPower * -Player.instance.lookDirection;
            velocity.y = jumpPower;
        }
    }

    private IEnumerator DownJump()
    {
        gameObject.layer = 11;
        velocity.y = 1;
        isProstrated = false;
        yield return new WaitForSeconds(0.3f);
        gameObject.layer = 9;
    }

    private void Climb()
    {
        onGround = false;
        isWalking = false;
        gameObject.layer = 11;
        gravity = 0;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(0, 0);
        velocity.x = 0;
        velocity.y = input.y * moveSpeed;
        animator.speed = Mathf.Abs(velocity.y);

        if (Input.GetKey(KeyCode.LeftAlt) && input.x != 0)
        {
            JumpOnClimb();
        }
    }

    private void JumpOnClimb()
    {
        onLadder = false;
        onRope = false;
        QuitClimb();
        velocity.x = input.x * 2;
        velocity.y = 2;
    }

    private void QuitClimb()
    {
        gravity = -9.81f;
        rb.gravityScale = 1;
        gameObject.layer = 9;
        animator.speed = 1;
    }

    private IEnumerator NormalAttack()
    {
        isAttacking = true;
        animator.SetTrigger("doAttack");
        Instantiate(normalAttackSkill);
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
    }

    private void SetAnimatorBool()
    {
        animator.SetBool("onGround", onGround);
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isProstrated", isProstrated);
        animator.SetBool("onLadder", onLadder);
        animator.SetBool("onRope", onRope);
    }
}
