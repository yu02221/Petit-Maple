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
        StartCoroutine(BtnBoardDown());
    }

    public void OnClickedContinueBtn()
    {
        StartCoroutine(BtnBoardDown());
    }

    public void OnClickedQuitButton()
    {
        Application.Quit();
    }

    IEnumerator BtnBoardDown()
    {
        BtnBoard.SetBool("goNextScene", true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Village");
    }
}
