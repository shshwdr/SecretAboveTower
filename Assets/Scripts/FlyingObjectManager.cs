using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlyingObjectManager : Singleton<FlyingObjectManager>
{
    // Start is called before the first frame update
    void Start()
    {
        
        //FlyingObjectManager.Instance.SpawnFlyingItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnFlyingItem()
    {
        var flyObjectPrefab = Resources.LoadAll<GameObject>("FlyingObjects").ToList().RandomItem();
        int x = 10;
        float y = 4;
        bool isLeft = Random.Range(0, 2) == 0;
        var finalX =  Camera.main.transform.position.x +(isLeft ? x : -x);
        var randomY = Random.Range(-y, y);
        var cameraY = Camera.main.transform.position.y;
        var startPosition = new Vector3(finalX, cameraY+randomY, 0);
        var finaly = cameraY + Random.Range(-2f, 2f);
        finaly = math.max(1, finaly);
        var endPosition = new Vector3(-finalX, finaly, 0);
        
        var flyObject = Instantiate(flyObjectPrefab, startPosition, Quaternion.identity,transform);
        flyObject.transform.DOMove(endPosition, 30f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(flyObject);
        });
        // flyObject.GetComponent<RectTransform>().DOMove(endPosition, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        // {
        //     flyObject.GetComponent<RectTransform>().DOMove(endPosition, 12f).SetEase(Ease.Linear).SetLoops(3, LoopType.Yoyo).OnComplete(
        //         () =>
        //         {
        //             flyObject.GetComponent<RectTransform>().DOMove(endPosition, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        //             {
        //                 
        //                 flyObject.gameObject.SetActive(false);
        //             });
        //             
        //         
        //         }); 
        // });
    }
}
