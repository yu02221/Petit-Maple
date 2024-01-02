using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject monster;
    public bool isSpawned = false;

    // 스폰포인트에서 몬스터 스폰
    public void Spawn()
    {
        isSpawned = true;
        monster.SetActive(true);
        monster.GetComponent<MonsterController>().Respawn();
        monster.transform.position = transform.position;
    }
}
