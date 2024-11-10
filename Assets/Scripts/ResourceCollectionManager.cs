using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public enum ResourceType
{
    love,
}
public struct  ResourceFlyObjData
{
    public Transform trans;
    public ResourceType resourceType;
    public int amount;
    public bool screenTrans;
    public string subType;
}
public class ResourceCollectionManager : Singleton<ResourceCollectionManager>
{
    public GameObject collectionPrefab;
    public GameObject collectionTextPrefab;
    IObjectPool<GameObject> resourceCollectionPool;
    // Collection checks will throw errors if we try to release an item that is already in the pool.
    public bool collectionChecks = true;
    public int maxPoolSize = 10;
    public Transform parent;

    List<ResourceFlyObjData> resourceFlyObjDatas = new List<ResourceFlyObjData>();
    // public void ShowResourceCollection(Transform trans, ResourceType resourceType, int amount,bool screenTrans = false,string subType = null)
    // {
    //     //ResourceTargetType targetType = Enum.Parse<ResourceTargetType>(resourceType.ToString());
    //     ShowResourceCollection(trans, resourceType, amount,screenTrans,subType);
    // }
    
    public void ShowResourceCollection(Transform trans, ResourceType resourceType,int amount,bool screenTrans = false,string subType = null)
    {
        if (resourceCollectionPool == null)
        {
            return;
        }

        var resourceFlyObjData = new ResourceFlyObjData()
        {
            trans = trans,
            resourceType = resourceType,
            amount = amount,
            screenTrans = screenTrans,
             subType = subType
        };
        resourceFlyObjDatas.Add(resourceFlyObjData);
    }

    private float flyTime = 0.1f;
    private float flyTimer = 0;
    
    private void Update()
    {
        flyTimer+= Time.deltaTime;
        if (resourceFlyObjDatas.Count > 0 && flyTimer>flyTime)
        {
            flyTimer = 0;
            var resourceFlyObjData = resourceFlyObjDatas[0];

            var screenPosition = resourceFlyObjData.trans.position;
            if (!resourceFlyObjData.screenTrans)
            {
                screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, resourceFlyObjData.trans.position);
            }
            
            //先随机显示1-20个
            int flyAmount = resourceFlyObjData.amount;
           // var resourceInfo = CSVLoader.Instance.getResourceInfo(resourceFlyObjData.resourceType.ToString());
            // if (resourceInfo!=null)
            // {
            //     flyAmount /= resourceInfo.flyDivident;
            // }
            //flyAmount = Random.Range(4, 10);
            flyAmount = math.clamp(flyAmount, 2, 20);
            for (int i = 0; i < flyAmount; i++)
            {
                var go = resourceCollectionPool.Get();
                go.transform.position = screenPosition;
                //var obj = Instantiate(collectionPrefab, go.transform);
                go.GetComponent<ResourceCollectionFlyingObject>().Init(resourceFlyObjData, 0.05f*i,screenPosition,Hud.Instance.gold.transform.position);
            }
            
            //go.GetComponent<ResourceCollectionFlyingObject>().Init(resourceFlyObjData.resourceType,resourceFlyObjData.resourceTargetType, resourceFlyObjData. amount);
            resourceFlyObjDatas.RemoveAt(0);
        }
            
            
    }

    // Start is called before the first frame update
    void Start()
    {
        resourceCollectionPool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
    }

    GameObject CreatePooledItem()
    {
        GameObject go = Instantiate(collectionPrefab,parent);
        return go;
    }
   
    // Called when an item is taken from the pool using Get
    void OnTakeFromPool(GameObject go)
    {
        go.gameObject.SetActive(true);
        go.GetComponent<PoolObject>().Init(resourceCollectionPool);
    }
// Called when an item is returned to the pool using Release
    void OnReturnedToPool(GameObject system)
    {
        system.gameObject.SetActive(false);
    }
    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    void OnDestroyPoolObject(GameObject system)
    {
        Destroy(system.gameObject);
    }
}
