using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    public GameObject MenuBar;

    private bool activeMenueBtn = false;

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

    public void OnClickMenuBtn()
    {
        activeMenueBtn = !activeMenueBtn;
        MenuBar.SetActive(activeMenueBtn);
    }

    public void OnClickQuitBtn()
    {
        PlayerPrefs.Save();
        activeMenueBtn = false;
        MenuBar.SetActive(false);
        Player.instance.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
