using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance = null;

    public int lookDirection = -1;   // left : -1, right : 1

    private Animator anim;

    private int level;
    private float exp;
    private float levelUpExp;
    public float Power { get; private set; }
    private float hp;
    private float maxHp;

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

        level = 1;
        exp = 0;
        levelUpExp = 10;
        Power = 10;
        maxHp = 100;
        hp = 100;
    }

    public void IncreaseExp(int monsterExp)
    {
        print($"{level}, {exp}");
        exp += monsterExp;
        if (exp >= levelUpExp)
            LevelUp();
    }

    private void LevelUp()
    {
        level++;
        exp = exp - levelUpExp;
        levelUpExp += level * 10;
        Power += 5;
        maxHp += level * 5;
        hp = maxHp;
    }

    public void Hurt(float damage)
    {
        print(hp);
        print(damage);
        if (hp > 0)
        {
            hp -= damage;

            if (hp <= 0)
                Die();
        }

    }
    private void Die()
    {
        exp -= levelUpExp * 0.1f;
        if (exp < 0)
            exp = 0;

        anim.SetTrigger("die");
        Instantiate(tombstone);
    }
}
