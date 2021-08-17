using Photon.Pun;
using UnityEngine;

namespace PacMan.Systems
{
    // A simple class that assists us in adding prefabs to the prefab pool. Allows us to not rely on the Unity Resources system when calling PhotonNetwork.Instantiate
    public static class PhotonObjectController
    {
        // Add to the prefab pool
        public static void AddPrefabToPool(GameObject prefab) 
        {
            if (prefab == null) return;
            if (!(PhotonNetwork.PrefabPool is DefaultPool pool)) return;
            if (pool.ResourceCache.ContainsKey(prefab.name)) return;
            
            pool.ResourceCache.Add(prefab.name, prefab);
        }
        
        // Remove from the prefab pool
        public static void RemovePrefabFromPool(GameObject prefab) 
        {
            if (prefab == null) return;
            if (!(PhotonNetwork.PrefabPool is DefaultPool pool)) return;
            if (pool.ResourceCache.ContainsKey(prefab.name)) return;
            
            pool.ResourceCache.Add(prefab.name, prefab);
        }
    }
}