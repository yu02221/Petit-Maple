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

    // 플레이어 스테이터스
    private string playerName;
    public int level;
    public int jobLevel = 0;
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
    public Text nameTagTxt;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI expTxt;
    public TextMeshProUGUI hpTxt;
    public TextMeshProUGUI mpTxt;
    public TextMeshProUGUI potionCountTxt;
    public TextMeshProUGUI mesoTxt;
    public TextMeshProUGUI damageTxt;
    public Animator damageTxtAnim;

    public GameObject tombstone;
    public GameObject deadWindow;
    public GameObject levelUpEffect;

    private AudioSource audioSrc;
    public AudioClip drinkPotionSnd;
    public AudioClip levelUpSnd;

    // 싱글톤
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

    // 플레이어 프리펩스에서 스테이터스 가져오기
    public void GetStatus()
    {
        playerName = PlayerPrefs.GetString("playerName");
        level = PlayerPrefs.GetInt("level");
        jobLevel = PlayerPrefs.GetInt("jobLevel");
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

    // 플레이어 프리펩스에 현제 스테이터스 저장
    private void SetStatus()
    {
        if (GameManager.instance.currentSceneNumber > 0)
            PlayerPrefs.SetInt("currentSceneNumber", GameManager.instance.currentSceneNumber);
        PlayerPrefs.SetString("playerName", playerName);
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("jobLevel", jobLevel);
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

    // 스테이터스에 맞춰 UI변경
    private void SetUI()
    {
        nameTxt.text = playerName;
        nameTagTxt.text = playerName;
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

    // 경험치 증가
    public void IncreaseExp(int monsterExp)
    {
        exp += monsterExp;
        if (exp >= maxExp)
            LevelUp();
    }

    // 메소 획득
    public void IncreaseMeso(int amount)
    {
        Meso += amount;
    }

    // 아이템 구매시 메소 감소
    public void DecreaseMeso(int amount)
    {
        Meso -= amount;
    }

    // 포션 소비
    public void IncreasePotionCount()
    {
        potionCount++;
    }
    
    // 레벨업 처리
    public void LevelUp()
    {
        GameObject lvUpEft = Instantiate(levelUpEffect);
        lvUpEft.transform.position = new Vector3(
            transform.position.x, transform.position.y - 0.16f, 0);
        Destroy(lvUpEft, 2.0f);

        audioSrc.clip = levelUpSnd;
        audioSrc.Play();

        level++;
        exp = (exp > maxExp) ? exp - maxExp : 0;
        maxExp += level * 10;
        Power += 5;
        maxHp += level * 5;
        hp = maxHp;
        maxMp += level * 2;
        Mp = maxMp;
    }

    // 피격 처리
    public void Hurt(float damage, float dir)
    {
        if (hp > 0 && !unbeatable)
        {
            hp -= damage;
            pc.HurtAction(dir);
            StartCoroutine(Unbeatable(0.5f));

            damageTxt.text = string.Format("{0:N0}", damage);
            damageTxtAnim.SetTrigger("hurt");

            if (hp <= 0)
            {
                hp = 0;
                Die();
            }
        }

    }

    // 사망 처리
    private void Die()
    {
        exp -= maxExp * 0.1f;
        if (exp < 0)
            exp = 0;
        pc.dead = true;
        Instantiate(tombstone);
        deadWindow.SetActive(true);
    }

    // 마을에서 부활
    public void Resurrection()
    {
        deadWindow.SetActive(false);
        hp = maxHp * 0.3f;
        pc.dead = false;
        GameManager.instance.GoToVillage();
    }
    
    // 공격 불가 코루틴
    private IEnumerator Unbeatable(float time)
    {
        unbeatable = true;
        yield return new WaitForSeconds(time);
        unbeatable = false;
    }

    // 포션을 통한 체력 회복
    private void DrinkPotion()
    {
        audioSrc.clip = drinkPotionSnd;
        audioSrc.Play();

        hp = maxHp;
        Mp = maxMp;
        potionCount--;
    }
}
