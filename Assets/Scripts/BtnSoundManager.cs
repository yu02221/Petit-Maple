using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
