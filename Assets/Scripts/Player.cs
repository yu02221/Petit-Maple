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
    public float Mp { get; set; }
    private float maxMp;

    private int potionCount = 10;
    public int Meso { get; private set; }

    private bool unbeatable = false;

    public float sliderSpeed;

    public Slider expSlider;
    public Slider hpSlider;
    public Slider mpSlider;
    public Text nameTxt;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI expTxt;
    public TextMeshProUGUI hpTxt;
    public TextMeshProUGUI mpTxt;
    public TextMeshProUGUI potionCountTxt;
    public TextMeshProUGUI mesoTxt;

    public GameObject tombstone;
    public GameObject deadWindow;
    public GameObject levelUpEffect;

    private AudioSource audioSrc;
    public AudioClip drinkPotionSnd;
    public AudioClip levelUpSnd;


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
        audioSrc = GetComponent<AudioSource>();

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

    public void GetStatus()
    {
        playerName = PlayerPrefs.GetString("playerName");
        level = PlayerPrefs.GetInt("level");
        exp = PlayerPrefs.GetFloat("exp");
        maxExp = PlayerPrefs.GetFloat("maxExp");
        Power = PlayerPrefs.GetFloat("power");
        maxHp = PlayerPrefs.GetFloat("maxHp");
        hp = PlayerPrefs.GetFloat("hp");
        maxMp = PlayerPrefs.GetFloat("maxMp");
        Mp = PlayerPrefs.GetFloat("mp");
        potionCount = PlayerPrefs.GetInt("potionCount");
        Meso = PlayerPrefs.GetInt("meso");
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
        PlayerPrefs.SetFloat("mp", Mp);
        PlayerPrefs.SetInt("potionCount", potionCount);
        PlayerPrefs.SetInt("meso", Meso);
        PlayerPrefs.Save();
    }

    private void SetUI()
    {
        nameTxt.text = "player";
        levelTxt.text = "Lv." + level;

        expTxt.text = string.Format("{0:N0}[{1:N2}%]",exp, exp / maxExp * 100f);
        hpTxt.text = string.Format("{0:N0} / {1:N0}", hp, maxHp);
        mpTxt.text = string.Format("{0:N0} / {1:N0}", Mp, maxMp);
        mesoTxt.text = Meso.ToString();
        potionCountTxt.text = potionCount.ToString();

        expSlider.value = Mathf.Lerp(expSlider.value, exp / maxExp, sliderSpeed * Time.deltaTime);
        hpSlider.value = Mathf.Lerp(hpSlider.value, hp / maxHp, sliderSpeed * Time.deltaTime);
        mpSlider.value = Mathf.Lerp(mpSlider.value, Mp / maxMp, sliderSpeed * Time.deltaTime);
    }

    public void IncreaseExp(int monsterExp)
    {
        exp += monsterExp;
        if (exp >= maxExp)
            LevelUp();
    }

    public void IncreaseMeso(int amount)
    {
        Meso += amount;
    }
    public void DecreaseMeso(int amount)
    {
        Meso -= amount;
    }
    public void IncreasePotionCount()
    {
        potionCount++;
    }

    public void LevelUp()
    {
        GameObject lvUpEft = Instantiate(levelUpEffect);
        lvUpEft.transform.position = new Vector3(
            transform.position.x, transform.position.y - 0.16f, 0);
        Destroy(lvUpEft, 2.0f);

        audioSrc.clip = levelUpSnd;
        audioSrc.Play();

        level++;
        exp = (exp > maxExp) ? maxExp - exp : 0;
        maxExp += level * 10;
        Power += 5;
        maxHp += level * 5;
        hp = maxHp;
        maxMp += level * 2;
        Mp = maxMp;
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
        audioSrc.clip = drinkPotionSnd;
        audioSrc.Play();

        hp = maxHp;
        Mp = maxMp;
        potionCount--;
    }
}
