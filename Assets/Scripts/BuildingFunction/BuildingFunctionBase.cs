using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFunctionBase : MonoBehaviour
{
    // Start is called before the first frame update
    public virtual void Trigger()
    {
        ResourceManager.Instance.AddGold(1);
    }
}
