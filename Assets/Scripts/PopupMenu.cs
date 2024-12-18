using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupMenu : MenuBase
{
    public TMP_Text title;
    public TMP_Text content;
    public Button closeButton;

    protected override void Awake()
    {
        base.Awake();
        closeButton.onClick.AddListener(() => Hide());
    }

    public  void Show(string title,string content)
    {
        this.title.text = title;
        this.content.text = content;
        Show();
    }
}
