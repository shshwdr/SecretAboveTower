using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkyObjectType{castle,debuff,destroy,rainbow,goodCastle,}
public class SkyObject : MonoBehaviour,IHoverable
{
    public SkyObjectType type;
    public bool used = false;


    public void Hover()
    {
        
        HoverOverMenu.FindFirstInstance<HoverOverMenu>().Show(type.ToString(),CSVLoader.Instance.skyObjectInfoDict[type.ToString()].desc);
    }
}
