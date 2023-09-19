using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 버튼에 마우스를 올리거나 클릭 시 사운드 출력
public class BtnSoundManager : MonoBehaviour
    , IPointerClickHandler
    , IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.instance.PlayBtnMouseOverSound();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.instance.PlayBtnMouseClickSound();
    }
}
