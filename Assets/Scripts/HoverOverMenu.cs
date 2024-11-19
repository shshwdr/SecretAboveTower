using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverOverMenu : MenuBase
{
    public TMP_Text title;
    public TMP_Text content;
    
    

    public  void Show(string title,string content)
    {
        this.title.text = title;
        this.content.text = content;
        Show();
    }
    
}
