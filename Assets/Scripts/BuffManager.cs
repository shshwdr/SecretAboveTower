using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : Singleton<BuffManager>
{
    public Dictionary<string, int> buffs = new Dictionary<string, int>();
    private int buffMaxLevel = 3;
    public void AddBuff(string name,int value=1)
    {
        if(buffs.ContainsKey(name))
        {
            buffs[name] += value;
        }
        else
        {
            buffs.Add(name,value);
        }

        BuffView.Instance.UpdateView();
    }

    public List<BuffInfo> GetAllDrawableBuffs()
    {
        var allCandidates = new List<BuffInfo>();
        foreach (var b in CSVLoader.Instance.buffInfoDict.Values)
        {
            if (b.canDraw)
            {
                if(!buffs.ContainsKey(b.identifier) || buffs[b.identifier] <buffMaxLevel)
                allCandidates.Add(b);
            }
        }

        return allCandidates;
    }
}
