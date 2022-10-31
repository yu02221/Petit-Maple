using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Animator BtnBoard;
    public void OnClickedNewStartBtn()
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
