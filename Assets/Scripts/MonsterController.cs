using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 몬스터 상태 목록
public enum States
{
    Idle,
    Move,
    Hurt,
    Angry,
    Die,
}

public class MonsterController : MonoBehaviour
{
    public States state;

    private SpriteRenderer sr;
    private Animator anim;

    private Player player;

    public SpawnPoint spawnPoint;

    public Slider HpBar;
    public TextMeshProUGUI damageTxt;
    public Animator damageTxtAnim;

    private AudioSource audioSrc;
    public AudioClip hurtSnd;
    public AudioClip dieSnd;

    public int level;
    public int exp;
    public int dropMeso;
    public float maxHp;
    public float hp;
    public float power;
    public float moveSpeed;
    private bool lookRight;
    private Vector2 velocity;

    public float leftMoveRange;     // 스폰 위치에 따른 왼쪽 최대 이동 범위
    public float rightMoveRange;    // 스폰 위치에 따른 오른쪽 최대 이동 범위

    private float randomMoveTime;
    private float moveTime;
    private float hurtTime;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();

        moveTime = 0;
        randomMoveTime = 1.0f;
    }

    private void Update()
    {
        SetMoveType();
        SelectAction();

        HpBar.value = Mathf.Lerp(HpBar.value, hp / maxHp, 5 * Time.deltaTime);
    }

    private void SetMoveType()
    {
        // 3 ~ 5초 마다 몬스터 행동 변경
        moveTime += Time.deltaTime;
        if (moveTime >= randomMoveTime)
        {
            moveTime = 0;
            randomMoveTime = Random.Range(3.0f, 5.0f);
            if (state == States.Move)
            {
                state = States.Idle;
                anim.SetBool("move", false);
            }
            else if (state == States.Idle)
            {
                state = States.Move;
                anim.SetBool("move", true);
                lookRight = (Random.value > 0.5f);
            }
        }
    }
    // 상태에 따른 행동 실행
    private void SelectAction()
    {
        switch (state)
        {
            case States.Idle:
                velocity.x = 0;
                break;
            case States.Move:
                Move();
                break;
            case States.Hurt:
                HurtAction();
                break;
            case States.Angry:
                Angry();
                break;
        }

        transform.Translate(velocity * Time.deltaTime);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, leftMoveRange, rightMoveRange),
            transform.position.y,
            transform.position.z);
    }

    private void Move()
    {
        if (transform.position.x <= leftMoveRange)
            lookRight = true;
        if (transform.position.x >= rightMoveRange)
            lookRight = false;
        if (lookRight)
        {
            sr.flipX = true;
            velocity.x = moveSpeed;
        }
        else
        {
            sr.flipX = false;
            velocity.x = -moveSpeed;
        }
    }
    // 피격시
    public void Hurt(float damage)
    {
        if (hp > 0)
        {
            hurtTime = 0;
            state = States.Hurt;
            anim.SetTrigger("hurt");
            hp -= damage;
            damageTxt.text = string.Format("{0:N0}", damage);
            damageTxtAnim.SetTrigger("hurt");
            audioSrc.clip = hurtSnd;
            audioSrc.Play();

            if (hp <= 0)
                StartCoroutine(Die());
        }
    }
    // 피격 시 뒤로 밀려난 후 Angry로 상태 변경
    private void HurtAction()
    {
        hurtTime += Time.deltaTime;
        if (hurtTime > 0.5f)
            state = States.Angry;

        velocity.x = Player.instance.lookDirection;
    }

    // 피격 후 플레이어 추격
    private void Angry()
    {
        anim.SetBool("move", true);
        Vector2 dir = player.transform.position - transform.position;
        if (dir.x > 0.5f)
        {
            sr.flipX = true;
            velocity.x = moveSpeed;
        }
        else if (dir.x < -0.5f)
        {
            sr.flipX = false;
            velocity.x = -moveSpeed;
        }
    }

    // 사망 애니메이션 출력 및 플레이어에게 보상 지급 후 디스폰
    private IEnumerator Die()
    {
        velocity.x = 0;
        state = States.Die;

        anim.SetTrigger("die");
        audioSrc.clip = dieSnd;
        audioSrc.Play();

        Player.instance.IncreaseExp(exp);
        Player.instance.IncreaseMeso(dropMeso);

        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
        spawnPoint.isSpawned = false;
    }

    public void Respawn()
    {
        hp = maxHp;
        state = States.Idle;
    }
}
