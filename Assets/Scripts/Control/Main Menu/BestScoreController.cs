using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestScoreController : MonoBehaviour
{
    public Text BestScore;

    protected void Start()
    {
        int bestScore = PlayerPrefs.GetInt("MOST_SURVIVED_DAYS");
        BestScore.text = Mathf.Max(0, bestScore).ToString();
    }
}
