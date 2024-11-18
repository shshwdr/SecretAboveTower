using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffSelectionCell : MonoBehaviour
{
    private BuffInfo info;
    public TMP_Text descText;
    public Button button;
    public Image buffImage;

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            BuffManager.Instance.AddBuff(info.identifier,1);
            FindObjectOfType<SelectBuffMenu>().Hide();
        });
    }

    public void UpdateCell(BuffInfo info)
    {
        this.info = info;
        buffImage.sprite = SpriteUtils.GetSynergySprite(info.subType);
        descText.text = info.desc;
        
    }
}