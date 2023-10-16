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
        // 소모 MP만큼 플레이어 Mp 감소
        Player.instance.Mp -= needMp;

        col = GetComponent<Collider2D>();
        if (col != null)
            StartCoroutine(RemoveCollider());

        // 플레이어 방향에 따라 스킬이 생성되는 위치 변경
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

    // 스킬 범위 내에 몬스터가 있을 경우 몬스테에게 데미지를 입힘
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            float rand = Random.Range(0.8f, 1.2f);
            collision.GetComponent<MonsterController>().Hurt(Player.instance.Power * damage * rand);
        }
    }

    // 스킬이 생성되고 일정 시간 후에 범위 제거
    IEnumerator RemoveCollider()
    {
        yield return new WaitForSeconds(0.1f);
        col.enabled = false;
    }
}
