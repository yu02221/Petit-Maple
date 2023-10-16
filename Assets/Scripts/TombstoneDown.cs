using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ÇÃ·¹ÀÌ¾î »ç¸Á½Ã ºñ¼® ¶³¾îÁü Ã³¸®
public class TombstoneDown : MonoBehaviour
{
    private Vector2 velocity;
    private float gravity = -9.81f;

    private void Start()
    {
        transform.position = new Vector3(
            Player.instance.transform.position.x,
            Player.instance.transform.position.y + 5f,
            Player.instance.transform.position.z);
    }

    private void Update()
    {
        if (transform.position.y > Player.instance.transform.position.y)
            velocity.y += gravity * Time.deltaTime;
        else
            velocity.y = 0;
        transform.Translate(velocity * Time.deltaTime);
    }
}
