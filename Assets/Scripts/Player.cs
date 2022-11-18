using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player instance = null;

    public int lookDirection = -1;   // left : -1, right : 1

    private Animator anim;

    private string playerName;
    private int level;
    private float exp;
    private float maxExp;
    public float Power { get; private set; }
    private float hp;
    private float maxHp;
    private float mp;
    private float maxMp;

    public Slider expSlider;
    public Slider hpSlider;
    public Slider mpSlider;
    public Text nameTxt;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI expTxt;
    public TextMeshProUGUI hpTxt;
    public TextMeshProUGUI mpTxt;

    public GameObject tombstone;

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

    private void Start()
    {
        anim = GetComponent<Animator>();

        GameManager.instance.ResetPlayerInfo("Player");
        GetStatus();
        
    }

    private void Update()
    {
        SetUI();
    }

    private void GetStatus()
    {
        playerName = PlayerPrefs.GetString("playerName");
        level = PlayerPrefs.GetInt("level");
        exp = PlayerPrefs.GetFloat("exp");
        maxExp = PlayerPrefs.GetFloat("maxExp");
        Power = PlayerPrefs.GetFloat("power");
        maxHp = PlayerPrefs.GetFloat("maxHp");
        hp = PlayerPrefs.GetFloat("hp");
        maxMp = PlayerPrefs.GetFloat("maxMp");
        mp = PlayerPrefs.GetFloat("mp");
    }

    private void SetUI()
    {
        nameTxt.text = "player";
        levelTxt.text = "Lv." + level;
        expTxt.text = string.Format("{0:N0}[{1:N2}]",exp, exp / maxExp * 100f);
        hpTxt.text = string.Format("{0:N0} / {1:N0}", hp, maxHp);
        mpTxt.text = string.Format("{0:N0} / {1:N0}", mp, maxMp);
        expSlider.value = exp / maxExp;
        hpSlider.value = hp / maxHp;
        mpSlider.value = mp / maxMp;
    }

    public void IncreaseExp(int monsterExp)
    {
        print($"{level}, {exp}");
        exp += monsterExp;
        if (exp >= maxExp)
            LevelUp();
    }

    private void LevelUp()
    {
        level++;
        exp -= maxExp;
        maxExp += level * 10;
        Power += 5;
        maxHp += level * 5;
        hp = maxHp;
        maxMp += level * 2;
        mp = maxMp;
    }

    public void Hurt(float damage)
    {
        if (hp > 0)
        {
            hp -= damage;

            if (hp <= 0)
                Die();
        }

    }
    private void Die()
    {
        exp -= maxExp * 0.1f;
        if (exp < 0)
            exp = 0;

        anim.SetTrigger("die");
        Instantiate(tombstone);
    }
}
