using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject monster;
    public bool isSpawned = false;

    // ��������Ʈ���� ���� ����
    public void MonsterSpawn()
    {
        isSpawned = true;
        monster.SetActive(true);
        monster.GetComponent<MonsterController>().Respawn();
        monster.transform.position = transform.position;
    }
}
