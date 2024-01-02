using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobChange : MonoBehaviour
{
    public GameObject questIcon;

    private void Update()
    {
        if ((Player.instance.jobLevel == 0 && Player.instance.level >= 10) ||
            (Player.instance.jobLevel == 1 && Player.instance.level >= 30) ||
            (Player.instance.jobLevel == 2 && Player.instance.level >= 60) ||
            (Player.instance.jobLevel == 3 && Player.instance.level >= 100))
            questIcon.SetActive(true);
        else
            questIcon.SetActive(false);
    }

    public void OnClickIcon()
    {
        Player.instance.jobLevel++;
        questIcon.SetActive(false);
    }
}
