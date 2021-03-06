﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClockGUI : MonoBehaviour
{
    Text text;
    public Text childText;
    GameTime instance;
    private int displayTime;

    void Awake()
    {
        text = GetComponentInChildren<Text>();
        GameManager.OnLevelLoaded += Prepare;
    }

    void Prepare()
    {
        //showTime = 3;
        //StopAllCoroutines();
        //childText.enabled = false;

        instance = GameTime.Instance;
        instance.OnTimeElapsed += SetTime;
        instance.OnTimeAdded += DisplayChild;
        childText.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        childText.enabled = false;
        showTime = 3;
       // GameManager.OnLevelLoaded -= Prepare;
    }

    void SetTime()
    {
        text.text = string.Format("{0:000}", instance.timeToFinish);
    }

    Coroutine coroutine = null;
    Color childColor = Color.white;
    float showTime = 3;
    IEnumerator Activate()
    {
        childText.enabled = true;
        childText.text = string.Format("+{0}", displayTime);
        childColor.a = 0;
        childText.color = childColor;
        while (childColor.a < 1)
        {
            childColor.a += 0.1f;
            childText.color = childColor;
            yield return null;
        }

        while (showTime > 0)
        {
            showTime--;
            yield return new WaitForSeconds(1);
        }

        while (childColor.a > 0)
        {
            childColor.a -= 0.1f;
            childText.color = childColor;
            yield return null;
        }
        coroutine = null;
        childColor.a = 0;
        childText.enabled = false;
        showTime = 3;
        yield return null;
    }

    void DisplayChild(int time)
    {
        displayTime = time;
        Effects.ScalePulse(childText.transform, 1, 1);
        if (coroutine == null || showTime == 0)
        {
            StopAllCoroutines();
            coroutine = StartCoroutine(Activate());
        }
        else
        {
            showTime = 3;
        }
    }
}