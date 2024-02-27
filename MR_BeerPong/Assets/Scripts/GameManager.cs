using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Start spawning the attributes on a selected table
    /// </summary>
    /// <param name="selectedTable"></param>
   public void StartSpawningAttributes(GameObject selectedTable)
   {
        ObjectSpawner[] spawners = selectedTable.GetComponentsInChildren<ObjectSpawner>();
        if (spawners.Length < 1) return;
        foreach(ObjectSpawner spawner in spawners)
        {
            if (spawner != null)
            {
                spawner.SpawnObjectAtRescaledSpawnPoint();
            }
        }       
   }
}
