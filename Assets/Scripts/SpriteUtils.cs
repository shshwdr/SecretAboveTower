using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteUtils
{
    public static Sprite GetSynergySprite(string name)
    {
        return Resources.Load<Sprite>("Synergy/"+name);
    }
}
