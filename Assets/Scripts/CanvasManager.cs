using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    public GameObject MenuBar;

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
        MenuBar.SetActive(!MenuBar.activeSelf);
    }

    public void OnClickQuitBtn()
    {
        PlayerPrefs.Save();
        MenuBar.SetActive(false);
        Player.instance.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
