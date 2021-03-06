﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverManager : MonoBehaviour
{
    [SerializeField] private Button OKButton;
    [SerializeField] private Text ScoreText;
    private void Start()
    {
        OKButton.onClick.AddListener(DataManager.GameDataDelete);
        OKButton.onClick.AddListener(SceneChanger.ToTitle);
        ScoreText.text=$"Score: {GameManager.GetTotalScore().ToString()}";
    }
	
}
