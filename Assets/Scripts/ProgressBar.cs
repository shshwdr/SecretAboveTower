using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image barImage;
    public  TMP_Text text;

    private int maxValue;

    public void SetMaxValue(int value)
    {
        maxValue = value;
    }
    public void SetProgress(float progress)
    {
        barImage.fillAmount = progress;
        text.text = (int)(progress)+"/"+maxValue;
    }
}
