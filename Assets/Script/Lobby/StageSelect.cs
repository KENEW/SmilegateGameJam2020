﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StageSelect : MonoBehaviour
{
    public GameObject scrollBar;

    public GameObject stageSelectPanel;
    public GameObject titlePanel;

    public Text stageNumText;
    public Text highScoreText;
    public Text stageNameText;
    public Text storyText;

    public Button rightButton;
    public Button leftButton;

    [SerializeField]
    private float scrollPos = 0;
    [SerializeField]
    private float[] contentPos;

    public int curStage = 0;

    public Vector2 selectScaleFalse = new Vector3(0.7f, 0.7f);
    public Vector2 selectScaleTrue = new Vector3(0.55f, 0.55f);

    public string[] stageStory = new string[3];
    public string[] stageName = new string[3];
    public int[] highScore = new int[3];


    private void Update()
    {
        contentPos = new float[transform.childCount];
        float distance = 1.0f / (contentPos.Length - 1.0f);

        for (int i = 0; i < contentPos.Length; i++)
        {
            contentPos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scrollPos = scrollBar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < contentPos.Length; i++)
            {
                if (scrollPos < (contentPos[i] + (distance / 2.0f)) && (scrollPos > contentPos[i] - (distance / 2.0f)))
                {
                    curStage = i;
                    UIUpdate();
                    stageNumText.text = (curStage + 1) + "";
                    scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, contentPos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < contentPos.Length; i++)
        {
            if (scrollPos < (contentPos[i] + (distance / 2.0f)) && (scrollPos > contentPos[i] - (distance / 2.0f)))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, selectScaleFalse, 0.1f);
                for (int a = 0; a < contentPos.Length; a++)
                {
                    transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, selectScaleTrue, 0.1f);
                }
            }
        }
    }

    public void GameStart()
    {
		SoundManager.Instance.PlaySFX("Button");
        LoadScene.Instance.LoadStart();
    }
    public void BackButton()
    {
        stageSelectPanel.SetActive(false);
        titlePanel.SetActive(true);
    }
    private void UIUpdate()
    {
        storyText.text = stageStory[curStage];
        highScoreText.text = highScore[curStage] + "";
        stageNameText.text = stageName[curStage];
    }
    public void NextStageButton()
    {
        if (curStage < contentPos.GetLength(0))
        {
            curStage++;
            rightButton.interactable = true;
            scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, contentPos[curStage], 0.1f);
        }
        else
        {
            rightButton.interactable = false;
        }
    }
    public void PrevStageButton()
    {
        if (curStage > 0)
        {
            curStage--;
            leftButton.interactable = false;
        }
        else
        {
            leftButton.interactable = false;
        }
    }
}
