using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class FlyingObject : MonoBehaviour
{
    public Transform flyingObjectInner;

    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        StartFloating();
        
        SFXManager.Instance.Play(clip);
    }
    
    void StartFloating()
    {
       // 生成一个随机的浮动目标位置
        float randomYOffset = Random.Range(-0.3f, 0.3f); // 随机上下浮动幅度
        float randomDuration = Random.Range(1f, 3f); // 随机浮动时间
        
        // 使用 DoTween 让图像在 Y 轴上浮动
        flyingObjectInner.DOLocalMoveY(flyingObjectInner.position.y + randomYOffset, randomDuration)
            .SetEase(Ease.InOutSine) // 使用平滑的 Sine 曲线让浮动更自然
            .SetLoops(-1, LoopType.Yoyo) // 无限循环来回浮动
            .OnStepComplete(() =>
            {
                // 每次浮动完成后随机更新浮动的偏移量和时间，使其看起来更加不规则
                randomYOffset = Random.Range(-0.3f, 0.3f);
                randomDuration = Random.Range(1f, 3f); 
                flyingObjectInner.DOLocalMoveY(flyingObjectInner.position.y + randomYOffset, randomDuration)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick()
    {
        ResourceManager.Instance.AddGold(transform,5);
        if (clip)
        {
            
            SFXManager.Instance.Play(clip);
        }
        Destroy(gameObject);
    }
    // private void OnMouseDown()
    // {
    //     // Perform actions on click (e.g., print a message or change color)
    //     Debug.Log("Object clicked!");
    //     ResourceManager.Instance.AddGold(5);
    //     Destroy(gameObject);
    //
    //     // You can also add other functionality here, such as:
    //     // - Change object color
    //     // - Trigger animations
    //     // - Destroy the object
    //     // - etc.
    // }
}
