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
    private PlayerController pc;

    private string playerName;
    private int level;
    private float exp;
    private float maxExp;
    public float Power { get; private set; }
    private float hp;
    private float maxHp;
    private float mp;
    private float maxMp;

    private int potionCount = 10;

    private bool unbeatable = false;

    public Slider expSlider;
    public Slider hpSlider;
    public Slider mpSlider;
    public Text nameTxt;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI expTxt;
    public TextMeshProUGUI hpTxt;
    public TextMeshProUGUI mpTxt;
    public TextMeshProUGUI potionCountTxt;

    public GameObject tombstone;
    public GameObject deadWindow;


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
        pc = GetComponent<PlayerController>();

        GetStatus();
        potionCountTxt.text = potionCount.ToString();
    }

    private void Update()
    {
        SetStatus();
        SetUI();

        if (potionCount > 0 && Input.GetKeyDown(KeyCode.Delete))
            DrinkPotion();
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
        potionCount = PlayerPrefs.GetInt("potionCount");
    }

    private void SetStatus()
    {
        PlayerPrefs.SetInt("currentSceneNumber", GameManager.instance.currentSceneNumber);
        PlayerPrefs.SetString("playerName", playerName);
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetFloat("exp", exp);
        PlayerPrefs.SetFloat("maxExp", maxExp);
        PlayerPrefs.SetFloat("power", Power);
        PlayerPrefs.SetFloat("maxHp", maxHp);
        PlayerPrefs.SetFloat("hp", hp);
        PlayerPrefs.SetFloat("maxMp", maxMp);
        PlayerPrefs.SetFloat("mp", mp);
        PlayerPrefs.SetInt("potionCount", potionCount);
        PlayerPrefs.Save();
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

    public void Hurt(float damage, float dir)
    {
        if (hp > 0 && !unbeatable)
        {
            hp -= damage;
            pc.HurtAction(dir);
            StartCoroutine(Unbeatable(0.5f));
            if (hp <= 0)
            {
                hp = 0;
                Die();
            }
        }

    }
    private void Die()
    {
        exp -= maxExp * 0.1f;
        if (exp < 0)
            exp = 0;
        pc.dead = true;
        Instantiate(tombstone);
        deadWindow.SetActive(true);
    }

    public void Resurrection()
    {
        deadWindow.SetActive(false);
        hp = maxHp * 0.3f;
        pc.dead = false;
        GameManager.instance.GoToVillage();
    }

    private IEnumerator Unbeatable(float time)
    {
        unbeatable = true;
        yield return new WaitForSeconds(time);
        unbeatable = false;
    }

    private void DrinkPotion()
    {
        hp = maxHp;
        mp = maxMp;
        potionCount--;
        potionCountTxt.text = potionCount.ToString();
    }
}
