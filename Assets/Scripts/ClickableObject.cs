using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClickableObject : MonoBehaviour
{
    public IClickable hoverable;

    private void Awake()
    {
        hoverable = Utils.FindInterfaceInParents<IClickable>(this);
    }

    public void Click()
    {
        hoverable.OnPointerClick();
    }
}
