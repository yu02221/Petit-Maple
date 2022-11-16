using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public float damage;
    public float cooltime;
    public float runtime;
    public float xPos;
    public float yPos;
    public float zRot;

    private void Start()
    {
        if (Player.instance.lookDirection == 1)
        {
            transform.position = new Vector3(
               Player.instance.transform.position.x + xPos,
               Player.instance.transform.position.y + yPos,
               0);
            transform.rotation = new Quaternion(0, 0, zRot, 0);
        }
        else if (Player.instance.lookDirection == -1)
        {
            transform.position = new Vector3(
               Player.instance.transform.position.x - xPos,
               Player.instance.transform.position.y + yPos,
               0);
            transform.rotation = new Quaternion(0, 180, zRot, 0);
        }
        
        Destroy(gameObject, runtime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            collision.GetComponent<MonsterController>().Hurt(damage);
        }
    }
}
