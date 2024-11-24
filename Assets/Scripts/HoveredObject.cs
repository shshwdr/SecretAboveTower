using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoverable
{
    public void Hover();
}



public class HoveredObject : MonoBehaviour
{
public IHoverable hoverable;

private void Awake()
{
    hoverable = Utils.FindInterfaceInParents<IHoverable>(this);
}

public void Show()
    {
        hoverable.Hover();
    }
}
