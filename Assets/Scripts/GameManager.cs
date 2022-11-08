using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject village;
    public GameObject forest;
    public void MapChange()
    {
        SceneManager.LoadScene("Field1");
    }
}
