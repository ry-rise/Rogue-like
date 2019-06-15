﻿using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour {
    private float fadeSpeed = 0.02f;        
    private float red, green, blue, alfa;
    private Image fadeImage;
    public bool isFadeOut = false;         
    public bool isFadeIn = false;
    
    private void Start()
    {
        fadeImage = GetComponent<Image>();
        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alfa = fadeImage.color.a;
    }

    private void Update()
    {
        if (isFadeOut)
        {
            StartFadeOut();
        }

        if (isFadeIn)
        {
            StartFadeIn();
        }
    }
    private void StartFadeIn()
    {
        alfa -= fadeSpeed;                
        SetAlpha();                      
        if (alfa <= 0)
        {                    
            isFadeIn = false;
            fadeImage.enabled = false;   
        }
    }
    private void StartFadeOut()
    {
        fadeImage.enabled = true; 
        alfa += fadeSpeed;         
        SetAlpha();               
        if (alfa >= 1)
        {
            isFadeOut = false;
        }
    }

    private void SetAlpha()
    {
        fadeImage.color = new Color(red, green, blue, alfa);
    }
}