using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

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

    int GetBuffValue(string key)
    {
        var level = buffs.GetValueOrDefault(key, 0);

        if (level <= 0)
        {
            return 0;
        }

        level -= 1;
        var buffInfo = CSVLoader.Instance.buffInfoDict[key];
        var values = buffInfo.values;
        level = math.min(level, values.Count - 1);
        return  values[level];
    }

    public bool canGetAnotherBuilding()
    {
        if (buffs.ContainsKey("chanceGetAnotherBuilding"))
        {
            if (Random.Range(0, 100) < GetBuffValue("chanceGetAnotherBuilding"))
            {
                 return true;
            }
        }

        return false;
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
