﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster1 : MonoBehaviour
{
    public Image hp;

    public GameObject bullet;
    public GameObject[] randomBullet;

    public Sprite[] monsterFace;
    private SpriteRenderer monsterFaceRenderer;
    private SpriteRenderer monsterRenderer;

    //private int monsterHp;
    public int monsterHp;
    private int monsterMaxHp = 3;

    public int monsterScore; //몬스터 피격 점수

    public float fadeTime = 0.9f;
    private bool isPlaying = false;

    public int waitingTime = 4;

    Vector2 bulletPos;

    void Start()
    {
        monsterRenderer = gameObject.GetComponent<SpriteRenderer>();
        monsterFaceRenderer = transform.Find("MonsterCanvas/Face").gameObject.GetComponent<SpriteRenderer>();

        bulletPos = new Vector2(transform.position.x, transform.position.y - gameObject.GetComponent<PolygonCollider2D>().bounds.extents.y - 0.45f);
        monsterHp = monsterMaxHp;
       
        StartCoroutine(BulletSpawn());
        StartCoroutine(SpawnRandomBullet());
    }
    public void Destroyed()
    {
        monsterFaceRenderer.sprite = monsterFace[2];
        transform.Find("MonsterCanvas/Hp").gameObject.SetActive(false);
        transform.Find("MonsterCanvas/HpBackground").gameObject.SetActive(false);

        transform.Find("MonsterCanvas/Face").gameObject.GetComponent<Animator>().SetTrigger("DieTrigger");
        gameObject.GetComponent<Animator>().SetTrigger("DieTrigger");
        Destroy(gameObject);
    }
    IEnumerator MonsterChangeFace(Sprite changeSprite)
    {
        monsterFaceRenderer.sprite = changeSprite;
        yield return new WaitForSeconds(1f);
        monsterFaceRenderer.sprite = monsterFace[0];
    }
    IEnumerator BulletSpawn()
    {
        while(true)
        {
            if(monsterHp>0)
            {
                yield return new WaitForSeconds(0.1f);
                Instantiate(bullet, bulletPos, Quaternion.identity);
                yield return new WaitForSeconds(waitingTime);
            }
        }
    }
    IEnumerator SpawnRandomBullet() //특수 투사체 생성
    {
        while(true)
        {
            int randTime = (int)Random.Range(5, 10);
            yield return new WaitForSeconds(randTime);
            Instantiate(randomBullet[Random.Range(0, 4)], bulletPos, Quaternion.identity);
        }
    }
    public void GetDamage(int hpValue)
    {
        if (monsterHp - hpValue <= 0)
        {
            monsterHp = 0;
            SoundManager.Instance.PlaySFX("MonsterDeathSFX");
            Destroyed();
        }
        else
        {
            monsterHp -= hpValue;
            SoundManager.Instance.PlaySFX("MonsterHitSFX");
            StartCoroutine(MonsterChangeFace(monsterFace[1])); //우는 표정
        }

        hp.fillAmount -= (float)hpValue / monsterMaxHp;
        Score.Instance.AddScore(monsterScore * hpValue); //몬스터 피격 시 점수 획득
    }
    public void RecoveryHp(int hpValue) //회복
    {
        if (monsterHp + hpValue >= monsterMaxHp)
        {
            monsterHp = monsterMaxHp;
        }
        else
        {
            monsterHp += hpValue;
        }

        hp.fillAmount += (float)hpValue / monsterMaxHp;
    }
    private void OnCollisionEnter2D(Collision2D coll) 
    {
        if (transform.CompareTag("RedMonster"))
        {
            if(coll.gameObject.tag=="RedBullet")
            {
                GetDamage(1);
                Debug.Log(monsterHp);
            }
        }
        else if (transform.CompareTag("GreenMonster"))
        {
            if (coll.gameObject.tag == "GreenBullet")
            {
                GetDamage(1);
                Debug.Log(monsterHp);
            }
        }
        else if (transform.CompareTag("BlueMonster"))
        {
            if (coll.gameObject.tag == "BlueBullet")
            {
                GetDamage(1);
                Debug.Log(monsterHp);
            }
        }  
    }
}
