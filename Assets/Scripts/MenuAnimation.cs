using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MenuAnimation : MonoBehaviour
{
    public MenuBase parentMenu;
    
    public MenuShowAnimType showAnimType;
    public RectTransform animatedRect;
    protected Vector3 startTrans;
    protected Vector3 targetTrans;
    protected float targetSizeDelta;
    private float showTime => parentMenu.showTime;
    private float hideTime => parentMenu.maxHideTime;
    private void Awake()
    {
        
        startTrans = animatedRect.position;
        targetTrans = animatedRect.position;
        
        targetSizeDelta = transform.localScale.x;
    }

    virtual public void ShowAnim()
    {
        switch (showAnimType)
        {
             case MenuShowAnimType.zoomOut:
                 
                 //animatedRect.position = startTrans;
                 animatedRect.localScale =Vector3.zero;
        
                 //animatedRect.DOMove(targetTrans, showTime).SetEase( Ease.InQuad);

                 // 缩放到目标大小
                 animatedRect.DOScale(targetSizeDelta, showTime).SetEase( Ease.OutBack);
                 break;
             case MenuShowAnimType.scrollUp:
                 
                 animatedRect.position = Camera.main.WorldToScreenPoint(Vector3.zero) - new Vector3(0, Screen.height/2,0);
                 //animatedRect.localScale =Vector3.zero;
        
                 animatedRect.DOMove(targetTrans, showTime).SetEase( Ease.InQuad);

                 // 缩放到目标大小
                 //animatedRect.DOScale(targetSizeDelta, showTime).SetEase( Ease.OutBack);
                 break;
        }
    }
    
    
    virtual public void Show()
    {

        if (animatedRect != null)
        {
            ShowAnim();
        }
    }

   
    virtual public void Hide(bool immediate = false)
    {
        //DOTween.KillAll();
        
        if (immediate)
        {
            return;
        }

        if (animatedRect != null)
        {

            switch (showAnimType)
            {
                case MenuShowAnimType.zoomOut:
                 
                    //animatedRect.DOMove(startTrans, hideTime).SetEase( Ease.OutQuad);

                    // 缩放到目标大小
                    animatedRect.DOScale(0, hideTime).SetEase( Ease.OutQuad);
                    break;
                case MenuShowAnimType.scrollUp:
                 
                    //animatedRect.localScale =Vector3.zero;
        
                    animatedRect.DOMove(Camera.main.WorldToScreenPoint(Vector3.zero) - new Vector3(0, Screen.height/2,0), hideTime).SetEase( Ease.OutQuad);

                    // 缩放到目标大小
                    //animatedRect.DOScale(targetSizeDelta, showTime).SetEase( Ease.OutBack);
                    break;
            }
        }

    }
    

}
