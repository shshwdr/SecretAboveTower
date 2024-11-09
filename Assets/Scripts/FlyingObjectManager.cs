using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlyingObjectManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnFlyingItem()
    {
        var flyObjectPrefab = Resources.LoadAll<GameObject>("FlyingObjects").ToList().RandomItem();
        //var flyObject = Instantiate(flyObjectPrefab, position, Quaternion.identity);
        
        // flyingObject.GetComponent<RectTransform>().DOMove(startPosition, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        // {
        //     flyingObject.GetComponent<RectTransform>().DOMove(endPosition, 12f).SetEase(Ease.Linear).SetLoops(3, LoopType.Yoyo).OnComplete(
        //         () =>
        //         {
        //             flyingObject.GetComponent<RectTransform>().DOMove(endPosition+new Vector3(offsetX,0,0), 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        //             {
        //                 
        //                 flyingObject.gameObject.SetActive(false);
        //             });
        //             
        //         
        //         }); 
        // });
    }
}
