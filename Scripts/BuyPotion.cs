using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NPC Ŭ�� �� �޼ҷ� ���� ����
public class BuyPotion : MonoBehaviour
{
    public AudioSource audioSrc;
    public AudioClip buyPotionSnd;  // ���� ���� ����
    public AudioClip lakeMesoSnd;   // ���� ���� ����

    public int potionPrice = 50;    // ���� ����

    private void OnMouseDown()
    {
        if (Player.instance.Meso >= potionPrice)
        {   // ����� �޼Ҹ� ������ �ִ� ���
            audioSrc.clip = buyPotionSnd;
            Player.instance.DecreaseMeso(potionPrice);
            Player.instance.IncreasePotionCount();

            audioSrc.Play();
        }
        else
        {   // �޼Ұ� ������ ���
            audioSrc.clip = lakeMesoSnd;
            audioSrc.Play();
        }
    }
}
