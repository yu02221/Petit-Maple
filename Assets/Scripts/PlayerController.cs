using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float gravity = -9.81f;
    public float jumpPower = 5.0f;

    private Vector3 velocity;

    public bool onGround = true;
    public bool isWalking = false;

    public SpriteRenderer sr;

    public Animator player;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.LeftAlt) && onGround)
        {
            NormalJump();
        }
    }

    private void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (onGround)
            MoveOnGround(input);
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        transform.Translate(velocity * Time.deltaTime);

    }

    private void MoveOnGround(Vector2 input)
    {
        velocity.x = input.x * moveSpeed;
        velocity.y = 0;
        isWalking = true;
        if (velocity.x > 0)
            sr.flipX = true;
        else if (velocity.x < 0)
            sr.flipX = false;
        else
            isWalking = false;

        player.SetBool("isWalking", isWalking);
    }

    private void NormalJump()
    {
        onGround = false;
        velocity.y = jumpPower;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }


}
