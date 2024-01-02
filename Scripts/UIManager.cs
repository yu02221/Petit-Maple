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

    // �����ϱ� ��ư Ŭ����
    public void OnClickedNewStartBtn()
    {
        inputNameWindow.SetActive(true);

        btnBoardAnim.SetTrigger("newGameSelected");

        inputNameWindow.SetActive(true);
    }

    // �̾��ϱ� ��ư Ŭ����
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

    // ������ ��ư Ŭ����
    public void OnClickedQuitButton()
    {
        Application.Quit();
    }

    // �÷��̾� �̸� Ȯ�� ��ư Ŭ����
    public void OnClickedNameConfirmButton()
    {
        string name = nameInputField.text;

        if (name == null || name.Length > 6 || name.Length < 2)
        {   // ���� �Ҹ����� �ʱ�ȭ
            alertWindow.SetActive(true);
            nameInputField.text = "";
        }
        else
        {   // ���� ������ �̸� ����
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

    // �÷��̾� �̸� �ۼ�â ��ҽ� ó��
    public void OnClickedNameCancleButton()
    {
        inputNameWindow.SetActive(false);
        btnBoardAnim.SetTrigger("cancled");
    }

    public void OnClickedAlertConfirmButton()
    {
        alertWindow.SetActive(false);
    }

    // �ִϸ��̼��� �����⸦ ��ٷȴٰ� �� ��ȯ
    IEnumerator WaitCompletAnim()
    {
        yield return new WaitForSeconds(1.8f);
        GameManager.instance.loadNewScene = true;
        SceneManager.LoadScene(PlayerPrefs.GetInt("currentSceneNumber"));
    }
}
