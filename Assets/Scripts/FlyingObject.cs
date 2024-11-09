using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FlyingObject : MonoBehaviour,IPointerClickHandler
{
    public GameObject flyingObject;
    public GameObject flyingObjectInner;
    // Start is called before the first frame update
    void Start()
    {
    }
    
    void StartFloating()
    {
        // 生成一个随机的浮动目标位置
        float randomYOffset = Random.Range(50f, 100f); // 随机上下浮动幅度
        float randomDuration = Random.Range(1f, 3f); // 随机浮动时间

        // 使用 DoTween 让图像在 Y 轴上浮动
        // flyingObjectInner.DOAnchorPosY(flyingObjectInner.anchoredPosition.y + randomYOffset, randomDuration)
        //     .SetEase(Ease.InOutSine) // 使用平滑的 Sine 曲线让浮动更自然
        //     .SetLoops(-1, LoopType.Yoyo) // 无限循环来回浮动
        //     .OnStepComplete(() =>
        //     {
        //         // 每次浮动完成后随机更新浮动的偏移量和时间，使其看起来更加不规则
        //         randomYOffset = Random.Range(50f, 100f);
        //         randomDuration = Random.Range(1f, 3f);
        //         flyingObjectInner.DOAnchorPosY(flyingObjectInner.anchoredPosition.y + randomYOffset, randomDuration)
        //             .SetEase(Ease.InOutSine)
        //             .SetLoops(-1, LoopType.Yoyo);
        //     });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(gameObject);
    }
}
