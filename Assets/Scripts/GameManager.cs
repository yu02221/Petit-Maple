using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private SpawnPoint[] spawnPoints;
    private float spawnTime = 1;

    public int currentSceneNumber;

    public bool loadNewScene = false;
    public bool loadLeftScene = false;
    public bool loadRightScene = false;

    private AudioSource audioSrc;
    public AudioClip btnMouseOverSnd;
    public AudioClip btnMouseClickSnd;

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
        audioSrc = GetComponent<AudioSource>();
    }


    private void Update()
    {
        currentSceneNumber = SceneManager.GetActiveScene().buildIndex;

        spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<SpawnPoint>();

        spawnTime -= Time.deltaTime;
        if (spawnTime < 0)
        {
            spawnTime = 10;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (!spawnPoints[i].isSpawned)
                {
                    spawnPoints[i].MonsterSpawn();
                }
            }
        }
    }
    public void ResetSpawnTime()
    {
        spawnTime = 1;
    }
    public void GoLeftField()
    {
        SceneManager.LoadScene(++currentSceneNumber);
        loadLeftScene = true;
        ResetSpawnTime();
    }
    public void GoRightField()
    {
        SceneManager.LoadScene(--currentSceneNumber);
        loadRightScene = true;
        ResetSpawnTime();
    }


    public void ResetPlayerInfo(string playerName)
    {
        PlayerPrefs.SetInt("currentSceneNumber", 1);
        PlayerPrefs.SetString("playerName", playerName);
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.SetFloat("exp", 0);
        PlayerPrefs.SetFloat("maxExp", 10);
        PlayerPrefs.SetFloat("power", 10);
        PlayerPrefs.SetFloat("maxHp", 100);
        PlayerPrefs.SetFloat("hp", 100);
        PlayerPrefs.SetFloat("maxMp", 50);
        PlayerPrefs.SetFloat("mp", 50);
        PlayerPrefs.SetInt("potionCount", 10);
        PlayerPrefs.SetInt("meso", 100);
        PlayerPrefs.Save();
    }

    public void GoToVillage()
    {
        loadNewScene = true;
        currentSceneNumber = 1;
        SceneManager.LoadScene(1);
    }

    public void PlayBtnMouseOverSound()
    {
        audioSrc.clip = btnMouseOverSnd;
        audioSrc.Play();
    }

    public void PlayBtnMouseClickSound()
    {
        audioSrc.clip = btnMouseClickSnd;
        audioSrc.Play();
    }
}
