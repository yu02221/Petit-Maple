using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill : MonoBehaviour
{
    public MonsterController mc;
    public int power;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (mc.state != States.Die && collision.tag == "Player")
        {
            float dir = (collision.transform.position - transform.position).normalized.x;
            collision.GetComponent<Player>().Hurt(power, dir);
        }
    }
}
