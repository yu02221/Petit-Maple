using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private void Start()
    {
        // 연속으로 포탈을 넘지 않기 위해 조건 확인
        string name = gameObject.name;
        if (name == "RightPortal" && GameManager.instance.loadLeftScene == true)
        {
            Player.instance.transform.position = transform.position;
            GameManager.instance.loadLeftScene = false;
        }
        if (name == "LeftPortal" && GameManager.instance.loadRightScene == true)
        {
            Player.instance.transform.position = transform.position;
            GameManager.instance.loadRightScene = false;
        }
    }
}
