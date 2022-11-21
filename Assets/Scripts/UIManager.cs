using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Animator BtnBoard;
    private string playerName;
    public void OnClickedNewStartBtn()
    {
        GameManager.instance.ResetPlayerInfo(playerName);
        if (Player.instance != null)
        {
            Player.instance.gameObject.SetActive(true);
            Player.instance.GetStatus();
        }
        GameManager.instance.loadNewScene = true;
        SceneManager.LoadScene(PlayerPrefs.GetInt("currentSceneNumber"));
    }

    public void OnClickedContinueBtn()
    {
        if (Player.instance != null)
        {
            Player.instance.gameObject.SetActive(true);
            Player.instance.GetStatus();
        }
        GameManager.instance.loadNewScene = true;
        SceneManager.LoadScene(PlayerPrefs.GetInt("currentSceneNumber"));
    }

    public void OnClickedQuitButton()
    {
        Application.Quit();
    }
}
