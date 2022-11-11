using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPortal : MonoBehaviour
{
    public Animator anim;

    public Transform destination;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("playerIn", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("playerIn", false);
        }
    }
}
