using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupMenu : MenuBase
{
    public TMP_Text title;
    public TMP_Text content;
    
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  void Show(string title,string content)
    {
        this.title.text = title;
        this.content.text = content;
        Show();
    }
}
