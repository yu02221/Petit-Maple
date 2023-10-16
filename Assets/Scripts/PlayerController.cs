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

    private float hurtTime;

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
    private bool isHurt = false;
    public bool dead;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    public LayerMask groundCheckLayerMask;

    private Transform hiddenPortalDest;

    public GameObject normalAttack;
    public GameObject plain;
    public GameObject marker;

    private AudioSource audioSrc;
    public AudioClip jumpSnd;
    public AudioClip normalAttackSnd;
    public AudioClip plainSnd;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim =  GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // 히든포탈 이용
        if (!dead && inHiddenPortal && Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position = hiddenPortalDest.position;
        }
    }

    // 플레이어 움직임 처리
    private void FixedUpdate()
    {
        if (!dead)
        {
            KeyboardInput();

            GroundCheck();

            Move();
        }
        else
            rb.velocity = new Vector2(0, 0);

        SetAnimatorBool();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!isAttacking)
        {
            // 사다리에 매다리기
            if (col.CompareTag("Ladder") && input.y > 0
            || col.CompareTag("LadderTop") && input.y < 0)
            {
                transform.position = new Vector3(
                    col.transform.position.x,
                    transform.position.y,
                    transform.position.z);
                onLadder = true;
            }
            // 줄에 매달리기
            if (col.CompareTag("Rope") && input.y > 0
                || col.CompareTag("RopeTop") && input.y < 0)
            {
                transform.position = new Vector3(
                    col.transform.position.x,
                    transform.position.y,
                    transform.position.z);
                onRope = true;
            }
            // 왼쪽 포탈로 이동
            if (col.name == "LeftPortal" && input.y > 0)
            {
                GameManager.instance.GoLeftField();
            }
            // 오른쪽 포탈로 이동
            if (col.name == "RightPortal" && input.y > 0)
            {
                GameManager.instance.GoRightField();
            }
            // 히든 포탈 안에 있는지 확인
            if (col.CompareTag("HiddenPortal"))
            {
                inHiddenPortal = true;
                hiddenPortalDest = col.gameObject.GetComponent<HiddenPortal>().destination;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // 사다리 탈출
        if (col.CompareTag("Ladder") && input.y <= 0
            || col.CompareTag("LadderTop") && input.y >= 0)
        {
            onLadder = false;
            QuitClimb();
        }
        // 로프 탈출
        if (col.CompareTag("Rope") && input.y <= 0
            || col.CompareTag("RopeTop") && input.y >= 0)
        {
            onRope = false;
            QuitClimb();
        }
        // 히든포탈 탈출
        if (col.CompareTag("HiddenPortal"))
            inHiddenPortal = false;
    }

    // 방향키 입력
    private void KeyboardInput()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"));

        if (!isAttacking && !isProstrated && !onLadder && !onRope)
        {
            if (input.x > 0)
            {
                sr.flipX = true;
                Player.instance.lookDirection = 1;
            }
            else if (input.x < 0)
            {
                sr.flipX = false;
                Player.instance.lookDirection = -1;
            }
        }
    }
    // 땅에 착지한 상태인지 판별
    private void GroundCheck()
    {
        onGround = Physics2D.OverlapCircle(
            transform.position, 0.1f, groundCheckLayerMask)
            && velocity.y <= 2.0f;

        if (onGround)
        {
            velocity.y = 0;
        }
    }

    // 플레이어 이동 처리
    private void Move()
    {
        if (rb.velocity.y < -0.2f)
            onGround = false;
        // 지면에 서 있을 때만 가능
        if (onGround)
        {
            if (!isProstrated && !isAttacking && !isHurt)
                Walking(input);

            if (!isProstrated && !isAttacking && !isHurt
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
            velocity.y += gravity * Time.fixedDeltaTime;
            velocity.y = Mathf.Clamp(velocity.y, 0, upJumpPower);
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

        if (Input.GetKey(KeyCode.A) && Player.instance.jobLevel >= 1 &&
            !isAttacking && !isProstrated && !onLadder && !onRope)
        {
            if (Player.instance.Mp >= plain.GetComponent<PlayerSkill>().needMp)
                StartCoroutine(Plain());
        }
        // 플레이어 이동
        transform.Translate(velocity * Time.fixedDeltaTime);
    }
    // 좌우 이동
    private void Walking(Vector2 input)
    {
        velocity.x = input.x * moveSpeed;
        isWalking = true;
        
        if (velocity.x == 0)
            isWalking = false;
    }
    // 일반 점프
    private void NormalJump()
    {
        audioSrc.clip = jumpSnd;
        audioSrc.Play();

        onGround = false;
        velocity.y = jumpPower;
    }

    // 아래 점프
    private IEnumerator DownJump()
    {
        audioSrc.clip = jumpSnd;
        audioSrc.Play();

        gameObject.layer = 11;
        velocity.y = 1;
        isProstrated = false;
        yield return new WaitForSeconds(0.3f);
        gameObject.layer = 9;
    }
    // 사다리 또는 줄 위일 때
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
        anim.speed = Mathf.Abs(velocity.y);

        if (Input.GetKey(KeyCode.LeftAlt) && input.x != 0)
        {
            JumpOnClimb();
        }
    }

    // 점프로 사다리 또는 줄 탈출
    private void JumpOnClimb()
    {
        audioSrc.clip = jumpSnd;
        audioSrc.Play();

        onLadder = false;
        onRope = false;
        QuitClimb();
        velocity.x = input.x * 2;
        velocity.y = 2;
    }

    // 사다리 또는 줄 탈출 처리
    private void QuitClimb()
    {
        gravity = -9.81f;
        rb.gravityScale = 1;
        gameObject.layer = 9;
        anim.speed = 1;
    }

    // 일반 공격 처리
    private IEnumerator NormalAttack()
    {
        audioSrc.clip = normalAttackSnd;
        audioSrc.Play();

        isAttacking = true;
        anim.SetTrigger("doAttack");
        yield return new WaitForSeconds(0.5f);
        Instantiate(normalAttack);
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    // 1차 스킬(플레인)
    private IEnumerator Plain()
    {
        audioSrc.clip = plainSnd;
        audioSrc.Play();

        isAttacking = true;
        anim.SetTrigger("doAttack");
        yield return new WaitForSeconds(0.5f);
        Instantiate(plain);
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    // 피격 모션 처리
    public void HurtAction(float dir)
    {
        anim.SetTrigger("hurt");
        rb.AddForce(new Vector2(dir * 100, 100));
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -2, 2), Mathf.Clamp(rb.velocity.y, -2, 2));
    }

    // 상태에 따라 애니메이션 변수 변경
    private void SetAnimatorBool()
    {
        anim.SetBool("onGround", onGround);
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isProstrated", isProstrated);
        anim.SetBool("onLadder", onLadder);
        anim.SetBool("onRope", onRope);
        anim.SetBool("die", dead);
    }
}
