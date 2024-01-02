using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NPC 클릭 시 메소로 포션 구매
public class BuyPotion : MonoBehaviour
{
    public AudioSource audioSrc;
    public AudioClip buyPotionSnd;  // 구매 성공 사운드
    public AudioClip lakeMesoSnd;   // 구매 싪패 사운드

    public int potionPrice = 50;    // 포션 가격

    private void OnMouseDown()
    {
        if (Player.instance.Meso >= potionPrice)
        {   // 충분한 메소를 가지고 있는 경우
            audioSrc.clip = buyPotionSnd;
            Player.instance.DecreaseMeso(potionPrice);
            Player.instance.IncreasePotionCount();

            audioSrc.Play();
        }
        else
        {   // 메소가 부족한 경우
            audioSrc.clip = lakeMesoSnd;
            audioSrc.Play();
        }
    }
}
