using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Start()
    {
        // 새 맵으로 이동 시 플레이어 스폰
        if (GameManager.instance.loadNewScene)
        {
            Player.instance.transform.position = transform.position;
            GameManager.instance.loadNewScene = false;
            GameManager.instance.ResetSpawnTime();
        }
    }
}
