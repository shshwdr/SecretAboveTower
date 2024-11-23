using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteUtils
{
    public static Sprite GetSynergySprite(string name)
    {
        return Resources.Load<Sprite>("Synergy/"+name);
    }

    public static Sprite GetBuffSprite(BuffInfo info)
    {
        
        return Resources.Load<Sprite>("Synergy/"+info.subType);
    }

    public static Sprite GetPopupMessageSprite(string identifier)
    {
        
            return Resources.Load<Sprite>("popupMessage/"+identifier);
    }
}
