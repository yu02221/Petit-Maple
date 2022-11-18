using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private int currentSceneNumber;

    public bool loadLeftScene = false;
    public bool loadRightScene = false;

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

    private void Start()
    {
        currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
    }

    public void GoLeftField()
    {
        SceneManager.LoadScene(++currentSceneNumber);
        loadLeftScene = true;
    }
    public void GoRightField()
    {
        SceneManager.LoadScene(--currentSceneNumber);
        loadRightScene = true;
    }

    public void ResetPlayerInfo(string playerName)
    {
        PlayerPrefs.SetString("playerName", playerName);
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.SetFloat("exp", 0);
        PlayerPrefs.SetFloat("maxExp", 10);
        PlayerPrefs.SetFloat("power", 10);
        PlayerPrefs.SetFloat("maxHp", 100);
        PlayerPrefs.SetFloat("hp", 100);
        PlayerPrefs.SetFloat("maxMp", 50);
        PlayerPrefs.SetFloat("mp", 50);
        PlayerPrefs.Save();
    }
}
