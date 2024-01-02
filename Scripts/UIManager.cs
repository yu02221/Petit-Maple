using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Animator btnBoardAnim;
    public GameObject inputNameWindow;
    public GameObject alertWindow;
    public InputField nameInputField;
    private string playerName;

    // 새로하기 버튼 클릭시
    public void OnClickedNewStartBtn()
    {
        inputNameWindow.SetActive(true);

        btnBoardAnim.SetTrigger("newGameSelected");

        inputNameWindow.SetActive(true);
    }

    // 이어하기 버튼 클릭시
    public void OnClickedContinueBtn()
    {
        if (Player.instance != null)
        {
            Player.instance.gameObject.SetActive(true);
            Player.instance.GetStatus();
        }
        btnBoardAnim.SetTrigger("newGameSelected");

        StartCoroutine(WaitCompletAnim());
    }

    // 끝내기 버튼 클릭시
    public void OnClickedQuitButton()
    {
        Application.Quit();
    }

    // 플레이어 이름 확인 버튼 클릭시
    public void OnClickedNameConfirmButton()
    {
        string name = nameInputField.text;

        if (name == null || name.Length > 6 || name.Length < 2)
        {   // 조건 불만족시 초기화
            alertWindow.SetActive(true);
            nameInputField.text = "";
        }
        else
        {   // 조건 만족시 이름 적용
            playerName = name;
            GameManager.instance.ResetPlayerInfo(playerName);
            if (Player.instance != null)
            {
                Player.instance.gameObject.SetActive(true);
                Player.instance.GetStatus();
            }
            GameManager.instance.loadNewScene = true;
            SceneManager.LoadScene(PlayerPrefs.GetInt("currentSceneNumber"));
        }
    }

    // 플레이어 이름 작성창 취소시 처리
    public void OnClickedNameCancleButton()
    {
        inputNameWindow.SetActive(false);
        btnBoardAnim.SetTrigger("cancled");
    }

    public void OnClickedAlertConfirmButton()
    {
        alertWindow.SetActive(false);
    }

    // 애니메이션이 끝나기를 기다렸다가 씬 전환
    IEnumerator WaitCompletAnim()
    {
        yield return new WaitForSeconds(1.8f);
        GameManager.instance.loadNewScene = true;
        SceneManager.LoadScene(PlayerPrefs.GetInt("currentSceneNumber"));
    }
}
