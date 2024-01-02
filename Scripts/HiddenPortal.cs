using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ��Ż ����
public class HiddenPortal : MonoBehaviour
{
    public Animator anim;

    public Transform destination;

    // ������ ��Ż�� �����ִ� ���� �ִϸ��̼� ���
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("playerIn", true);
        }
    }
    // ��Ż���� ��� �� �ִϸ��̼� ����
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("playerIn", false);
        }
    }
}
