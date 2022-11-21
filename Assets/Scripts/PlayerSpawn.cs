using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.instance.loadNewScene)
        {
            Player.instance.transform.position = transform.position;
            GameManager.instance.loadNewScene = false;
            GameManager.instance.ResetSpawnTime();
        }
    }
}
