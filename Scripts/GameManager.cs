using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private SpawnPoint[] spawnPoints;   // 각 필드의 몬스터 스폰 위치
    private float spawnTime = 1;        // 필드 진입 시 최초 스폰 대기 시간

    public int currentSceneNumber;      // 현제 씬 넘버

    // 포탈 이동 시 연속 이동 방지
    public bool loadNewScene = false;
    public bool loadLeftScene = false;
    public bool loadRightScene = false;

    private AudioSource audioSrc;
    public AudioClip btnMouseOverSnd;
    public AudioClip btnMouseClickSnd;

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

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }


    private void Update()
    {
        // 현제 씬 넘버 최신화
        currentSceneNumber = SceneManager.GetActiveScene().buildIndex;

        // 현제 필드의 스폰 포인트로 변경
        spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<SpawnPoint>();

        spawnTime -= Time.deltaTime;
        if (spawnTime < 0)
        {   // 최초 스폰 후 몬스터가 사라진 스폰포인트에서 10초에 한 번씩 몬스터 스폰
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
    // 최초 스폰 시간으로 리셋
    public void ResetSpawnTime()
    {
        spawnTime = 1;
    }
    // 왼쪽 필드로 이동
    public void GoLeftField()
    {
        SceneManager.LoadScene(++currentSceneNumber);
        loadLeftScene = true;
        ResetSpawnTime();
    }
    // 오른쪽 필드로 이동
    public void GoRightField()
    {
        SceneManager.LoadScene(--currentSceneNumber);
        loadRightScene = true;
        ResetSpawnTime();
    }

    // 새로하기 선택 시 플레이어 이름 저장 및 플레이어 정보 리셋
    public void ResetPlayerInfo(string playerName)
    {
        PlayerPrefs.SetInt("currentSceneNumber", 1);
        PlayerPrefs.SetString("playerName", playerName);
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.SetInt("jobLevel", 0);
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
    // 플레이어 사망시 마을로 이동
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
