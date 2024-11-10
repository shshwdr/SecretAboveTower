using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
   private int gold;

   public void AddGold(Transform transform, int amount)
   {
      gold += amount;
      
      ResourceCollectionManager.Instance.ShowResourceCollection(transform, ResourceType.love, amount);
   }
   public void RemoveGold(int amount)
   {
      gold -= amount;
   }
   public int GetGold()
   {
      return gold;
   }
   public void Init()
   {
      gold = 0;
   }
}
