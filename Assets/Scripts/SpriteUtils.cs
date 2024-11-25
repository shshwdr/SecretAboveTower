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
        
        return Resources.Load<Sprite>("buff/"+info.icon);
    }

    public static Sprite GetPopupMessageSprite(string identifier)
    {
        
            return Resources.Load<Sprite>("popupMessage/"+identifier);
    }

    public static Sprite GetBuildingSprite(BuildingInfo info)
    {
        return Resources.Load<Sprite>("BuildingsSprite/" + info.image);
    }
}
