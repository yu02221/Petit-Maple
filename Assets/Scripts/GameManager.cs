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
}
