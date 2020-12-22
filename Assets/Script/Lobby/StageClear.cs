﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StageClear : MonoBehaviour
{
    public LightLoading lightLoading;

    public Text remainTimeText;
    public Text remainHealthText;
    public Text totalScoreText;

    private int remainTime = 0;
    private int remainHealth = 0;
    private int acquireScore = 0;
    private int totalScore = 0;

    private int SET_HEALTH_UP = 50;     //점수로 변환 기준
    private int SET_RE_TIME_UP = 30;

    private void Update()
    {
        //Test
        if (Input.GetKeyDown(KeyCode.T))
        {
            InitScore();
            GetaquireScore(5000);
            GetRemainTime(60);
            GetRemainHealth(3);

            ScoreResult();
        }
    }
    public void OnNextStage()   //다음 스테이지
    {
        lightLoading.LoadStart(SceneManager.GetActiveScene().name);
    }
    public void GetRemainTime(int time) //남은 시간 받아오기
    {
        remainTime = time;
    }
    public void GetRemainHealth(int health) //남은 체력 받아오기
    {
        remainHealth = health;
    }
    public void GetaquireScore(int score)   //게임하는 도중에서 얻은 스코어
    {
        acquireScore = score;
    }
    private void InitScore()    //얻은 점수 초기화
    {
        remainTime = 0;
        remainHealth = 0;
        acquireScore = 0;
        totalScore = 0;
    }
    private void ScoreResult()  //스코어 계산 ( 점수 계산 시작)
    {
        totalScore += remainHealth * SET_HEALTH_UP;
        totalScore += remainTime * SET_RE_TIME_UP;
        totalScore += acquireScore;

        StartCoroutine(CountSequence());
    }
    private IEnumerator CountSequence() //숫자 카운팅 시퀀스
    {
        float delay = 0.4f;

        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    StartCoroutine(Count(remainTimeText, remainTime, 0));
                    yield return new WaitForSeconds(delay);
                    break;
                case 1:
                    StartCoroutine(Count(remainHealthText, remainHealth, 0));
                    yield return new WaitForSeconds(delay);
                    break;
                case 2:
                    StartCoroutine(Count(totalScoreText, totalScore, acquireScore));
                    yield return new WaitForSeconds(delay);
                    break;
            }
        }
    }
    private IEnumerator Count(Text countText, float target, float current)
    {
        float duration = 0.5f; // Duration Counting TIme
        float offset = (target - current) / duration;

        while (current < target)
        {
            current += offset * Time.deltaTime;
            countText.text = ((int)current).ToString();

            yield return null;
        }

        current = target;
        countText.text = ((int)current).ToString();
    }
}