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
        GameManager.instance.loadNewScene = true;
        SceneManager.LoadScene(PlayerPrefs.GetInt("currentSceneNumber"));
    }

    public void OnClickedContinueBtn()
    {
        Player.instance.gameObject.SetActive(true);
        GameManager.instance.loadNewScene = true;
        SceneManager.LoadScene(PlayerPrefs.GetInt("currentSceneNumber"));
    }

    public void OnClickedQuitButton()
    {
        Application.Quit();
    }
}
