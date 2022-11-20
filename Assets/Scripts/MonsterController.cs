using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int level = 5;
    public int exp = 5;
    public float maxHp = 15;
    public float hp = 15;
    public float power = 10;
    public float moveSpeed = 0.5f;
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

        moveTime = 0;
        randomMoveTime = 1.0f;
    }

    private void Update()
    {
        SetMoveType();
        SelectAction();
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

            if (hp <= 0)
                Die();
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

    private void Die()
    {
        velocity.x = 0;
        state = States.Die;
        anim.SetTrigger("die");
        Player.instance.IncreaseExp(exp);
        Destroy(gameObject, 1.0f);
    }
}
