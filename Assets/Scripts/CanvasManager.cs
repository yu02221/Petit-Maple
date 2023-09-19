using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    public GameObject MenuBar;

    // 싱글톤
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 메뉴 바 활성화, 비활성화
    public void OnClickMenuBtn()
    {
        MenuBar.SetActive(!MenuBar.activeSelf);
    }

    // 저장 후 메뉴 씬으로 이동
    public void OnClickQuitBtn()
    {
        PlayerPrefs.Save();
        MenuBar.SetActive(false);
        Player.instance.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
