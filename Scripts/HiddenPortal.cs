using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 숨겨진 포탈 구현
public class HiddenPortal : MonoBehaviour
{
    public Animator anim;

    public Transform destination;

    // 숨겨진 포탈에 겹쳐있는 동안 애니메이션 재생
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("playerIn", true);
        }
    }
    // 포탈에서 벗어날 때 애니메이션 종료
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("playerIn", false);
        }
    }
}
