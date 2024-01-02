using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill : MonoBehaviour
{
    public MonsterController mc;
    public int power;

    // �÷��̾ ������ �÷��̾�� power ��ŭ �������� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (mc.state != States.Die && collision.tag == "Player")
        {
            float rand = Random.Range(0.8f, 1.2f);
            float dir = (collision.transform.position - transform.position).normalized.x;
            collision.GetComponent<Player>().Hurt(power * rand, dir);
        }
    }
}