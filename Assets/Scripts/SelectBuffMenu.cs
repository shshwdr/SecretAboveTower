using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectBuffMenu : MenuBase
{
    public TMP_Text title;

    public void SetMilestoneTitle()
    {
        title.text = "You reached a milestone, Select a buff!";
    }
}
