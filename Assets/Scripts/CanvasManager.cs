using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    public GameObject MenuBar;

    public GameObject PlainIcon;

    // �̱���
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

    private void Update()
    {
        if (Player.instance.jobLevel >= 1)
            PlainIcon.SetActive(true);
        else
            PlainIcon.SetActive(false);
    }

    // �޴� �� Ȱ��ȭ, ��Ȱ��ȭ
    public void OnClickMenuBtn()
    {
        MenuBar.SetActive(!MenuBar.activeSelf);
    }

    // ���� �� �޴� ������ �̵�
    public void OnClickQuitBtn()
    {
        PlayerPrefs.Save();
        MenuBar.SetActive(false);
        Player.instance.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
