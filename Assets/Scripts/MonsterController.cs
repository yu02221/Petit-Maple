using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States
{
    Idle,
    Move,
    Hurt,
    Die,
}

public class MonsterController : MonoBehaviour
{
    public States state;

    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb;

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

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        moveTime = 0;
        randomMoveTime = 1.0f;
    }

    private void Update()
    {
        SetMoveType();
        Move();
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

    private void Move()
    {
        if (state == States.Idle)
        {
            velocity.x = 0;
        }
        else if (state == States.Move)
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
        else if (state == States.Hurt)
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
                velocity.x =  -moveSpeed;
            }
        }
        transform.Translate(velocity * Time.deltaTime);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, leftMoveRange, rightMoveRange),
            transform.position.y,
            transform.position.z);
        
    }

    public void Hurt(float damage)
    {
        if (hp > 0)
        {
            state = States.Hurt;
            anim.SetTrigger("hurt");
            hp -= damage;
        }

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        velocity.x = 0;
        state = States.Die;
        anim.SetTrigger("die");
        Destroy(gameObject, 1.0f);
    }
}