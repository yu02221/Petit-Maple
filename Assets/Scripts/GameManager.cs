using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject village;
    public GameObject forest;
    public void MapChange()
    {
        forest.SetActive(true);
        village.SetActive(false);
    }
}
