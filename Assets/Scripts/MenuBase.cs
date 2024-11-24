using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public enum MenuShowAnimType{zoomOut,scrollUp,scrollDown}
public class MenuBase : MonoBehaviour
{
    public GameObject menu;
    protected Image blockImage;
    public bool IsActive => menu.activeInHierarchy;
    protected bool isInteracting = false;

[Header("Menu Animation")]
[Header("Fade In&Out")]
    public bool fadeInAndOut = true;
    public CanvasGroup fadeInAndOutRect;
    private float fadeInAndOutTime = 0.15f;
    [Header("Show & Hide")] 
    public RectTransform animatedRect;
    protected Vector3 startTrans;
    protected Vector3 targetTrans;
    protected float targetSizeDelta;
    private float hideTime = 0.3f;
    public  float showTime = 0.5f;
    [HideInInspector]
    public float maxHideTime;
    public MenuShowAnimType showAnimType;
    protected float autoInteractTime = 1;
    public bool wouldPause = true;
    private float timeScale;
    public virtual void UpdateMenu()
    {
        
    }
    
    
    protected virtual void Awake()
    {
        maxHideTime = 0;
        if (fadeInAndOut)
        {
            maxHideTime = fadeInAndOutTime;
        }

        if (animatedRect != null)
        {
            maxHideTime = math.max(hideTime, fadeInAndOutTime);
        }
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        menu.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        blockImage = menu.GetComponent<Image>();
        if (blockImage)
        {
            
            var color = menu.GetComponent<Image>().color;
            color.a = 0.7f;
            menu.GetComponent<Image>().color = color;
        }
        
        targetTrans = transform.position;
        targetSizeDelta = transform.localScale.x;

        if (fadeInAndOutRect == null)
        {
            fadeInAndOutRect = GetComponent<CanvasGroup>();
        }
        
        if (fadeInAndOutRect != null && fadeInAndOut)
        {
            fadeInAndOutRect.alpha = 0;
        }

        startTrans = Camera.main.WorldToScreenPoint(Vector3.zero);

    }

    public virtual void testInteractAndAct()
    {
        
        if (isInteracting)
        {
            return;
        }
        isInteracting = true;
        StartCoroutine((waitAndInteract()));
    }

    public virtual IEnumerator waitAndInteract()
    {
        yield return new WaitForSeconds(autoInteractTime);
        tryInteract();
        isInteracting = false;
    }

    public virtual void tryInteract()
    {
        
    }
    
    public static T FindFirstInstance<T>() where T : MenuBase
    {
        T instance = FindObjectOfType<T>();
        if (instance == null)
        {
            Debug.LogWarning($"No instance of {typeof(T).Name} found in the scene.");
        }
        return instance;
    }
    public static void OpenMenu<T>() where T : MenuBase
    {
        var instance = FindFirstInstance<T>();
        if (instance != null)
        {
            instance.Show();
        }
    }
    protected virtual void Start()
    {
        Hide(true);
    }

    virtual public void Init()
    {
    }
    virtual public void ShowAnim(bool immediate)
    {
        switch (showAnimType)
        {
             case MenuShowAnimType.zoomOut:
                 if (immediate)
                 {
                     animatedRect.position = targetTrans;
                     animatedRect.localScale = new Vector3(1,1,1) * targetSizeDelta;
                 }
                 else
                 {
                     
                     animatedRect.position = startTrans;
                     animatedRect.localScale =Vector3.zero;
        
                     animatedRect.DOMove(targetTrans, showTime).SetEase( Ease.OutBack).SetUpdate(true);

                     // 缩放到目标大小
                     animatedRect.DOScale(targetSizeDelta, showTime).SetEase( Ease.OutBack).SetUpdate(true);
                 }
                 break;
             case MenuShowAnimType.scrollUp:
                 
                 if (immediate)
                 {
                     animatedRect.position = targetTrans;
                 }
                 else
                 {
                     
                     animatedRect.position = Camera.main.WorldToScreenPoint(Vector3.zero) - new Vector3(0, Screen.height/2,0);
                     //animatedRect.localScale =Vector3.zero;
        
                     animatedRect.DOMove(targetTrans, showTime).SetEase( Ease.InQuad).SetUpdate(true);

                     // 缩放到目标大小
                     //animatedRect.DOScale(targetSizeDelta, showTime).SetEase( Ease.OutBack);
                 }
                 break;
             case MenuShowAnimType.scrollDown:
                 
                 if (immediate)
                 {
                     animatedRect.position = targetTrans;
                 }
                 else
                 {
                     
                     animatedRect.position = Camera.main.WorldToScreenPoint(Vector3.zero) + new Vector3(0, Screen.height/2,0);
                     //animatedRect.localScale =Vector3.zero;
        
                     animatedRect.DOMove(targetTrans, showTime).SetEase( Ease.InQuad).SetUpdate(true);

                     // 缩放到目标大小
                     //animatedRect.DOScale(targetSizeDelta, showTime).SetEase( Ease.OutBack);
                 }
                 break;
        }
    }

    private bool isAnimating = false;
    virtual public void Show(bool immediate = false)
    {
        if (wouldPause)
        {
            timeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        StopAllCoroutines();
        menu.SetActive(true);

        
        
        if (fadeInAndOutRect != null && fadeInAndOut)
        {
            
            if (immediate)
            {
                fadeInAndOutRect.alpha = 1;
            }
            else
            {
                fadeInAndOutRect.DOKill();
                
                fadeInAndOutRect.alpha = 0;
                fadeInAndOutRect.DOFade(1, fadeInAndOutTime).SetUpdate(true);
            }
        }

        if (animatedRect != null)
        {
            animatedRect.DOKill();
            ShowAnim(immediate);
        }
        
        foreach (var animation in GetComponentsInChildren<MenuAnimation>())
        {
            animation.Show();
        }
    }

   
    virtual public void Hide(bool immediate = false)
    {
        //DOTween.KillAll();
        if (wouldPause)
        {

            Time.timeScale = 1;//timeScale;
        }
        foreach (var animation in GetComponentsInChildren<MenuAnimation>())
        {
            animation.Hide(immediate);
        }
        if (immediate)
        {
            finalHide();
            return;
        }
        if (fadeInAndOutRect != null && fadeInAndOut)
        {
            fadeInAndOutRect.DOKill();
            fadeInAndOutRect.DOFade(1, fadeInAndOutTime).SetUpdate(true);
            
        }

        if (animatedRect != null)
        {

            animatedRect.DOKill();
            switch (showAnimType)
            {
                case MenuShowAnimType.zoomOut:
                 
                    animatedRect.DOMove(startTrans, hideTime).SetEase( Ease.OutQuad);

                    // 缩放到目标大小
                    animatedRect.DOScale(0, hideTime).SetEase( Ease.OutQuad).SetUpdate(true);
                    break;
                case MenuShowAnimType.scrollUp:
                 
                    //animatedRect.localScale =Vector3.zero;
        
                    animatedRect.DOMove(Camera.main.WorldToScreenPoint(Vector3.zero) - new Vector3(0, Screen.height/2,0), hideTime).SetEase( Ease.OutQuad);

                    // 缩放到目标大小
                    //animatedRect.DOScale(targetSizeDelta, showTime).SetEase( Ease.OutBack);
                    break;
                case MenuShowAnimType.scrollDown:
                 
                    //animatedRect.localScale =Vector3.zero;
        
                    animatedRect.DOMove(Camera.main.WorldToScreenPoint(Vector3.zero) + new Vector3(0, Screen.height/2,0), hideTime).SetEase( Ease.OutQuad);

                    // 缩放到目标大小
                    //animatedRect.DOScale(targetSizeDelta, showTime).SetEase( Ease.OutBack);
                    break;
            }
        }
        StartCoroutine(HideWithDelay(maxHideTime));

    }
    

    IEnumerator HideWithDelay(float delay)
    {
         yield return new WaitForSeconds(delay);
         finalHide();
    }

    void finalHide()
    {
        
        menu.SetActive(false);
    }
}
