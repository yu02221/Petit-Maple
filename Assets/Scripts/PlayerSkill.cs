using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public float damage;
    public float needMp;
    public float cooltime;
    public float runtime;
    public float xPos;
    public float yPos;
    public float zRot;

    private Collider2D col;

    private void Start()
    {
        // �Ҹ� MP��ŭ �÷��̾� Mp ����
        Player.instance.Mp -= needMp;

        col = GetComponent<Collider2D>();
        if (col != null)
            StartCoroutine(RemoveCollider());

        // �÷��̾� ���⿡ ���� ��ų�� �����Ǵ� ��ġ ����
        if (Player.instance.lookDirection == -1)
        {
            transform.position = new Vector3(
               Player.instance.transform.position.x + xPos,
               Player.instance.transform.position.y + yPos,
               0);
            transform.localEulerAngles = new Vector3(0, 0, zRot);
        }
        else if (Player.instance.lookDirection == 1)
        {
            transform.position = new Vector3(
               Player.instance.transform.position.x - xPos,
               Player.instance.transform.position.y + yPos,
               0);
            transform.localEulerAngles = new Vector3(0, 180, zRot);
        }
        
        Destroy(gameObject, runtime);
    }

    // ��ų ���� ���� ���Ͱ� ���� ��� ���׿��� �������� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            float rand = Random.Range(0.8f, 1.2f);
            collision.GetComponent<MonsterController>().Hurt(Player.instance.Power * damage * rand);
        }
    }

    // ��ų�� �����ǰ� ���� �ð� �Ŀ� ���� ����
    IEnumerator RemoveCollider()
    {
        yield return new WaitForSeconds(0.1f);
        col.enabled = false;
    }
}
