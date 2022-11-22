using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public float leftMoveRange;
    public float rightMoveRange;

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

    private void HurtAction()
    {
        hurtTime += Time.deltaTime;
        if (hurtTime > 0.5f)
            state = States.Angry;

        velocity.x = Player.instance.lookDirection;
    }

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
