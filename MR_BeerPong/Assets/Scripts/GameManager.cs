using System;
using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private bool _applicationLockState = false;
    public bool applicationLockState => _applicationLockState;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    /// <summary>
    /// Start spawning the attributes on a selected table
    /// </summary>
    /// <param name="selectedTable"></param>
    public void StartSpawningAttributes(GameObject selectedTable)
    {
        ObjectSpawner[] spawners = selectedTable.GetComponentsInChildren<ObjectSpawner>();
        if (spawners.Length < 1) return;
        foreach (ObjectSpawner spawner in spawners)
        {
            if (spawner != null)
            {
                spawner.SpawnObjectAtRescaledSpawnPoint();
            }
        }
    }

    /// <summary>
    /// Lock or unlock the application flow.
    /// This manager will only hold the value.
    /// The implementation of this value depends on how it is used in other scripts.
    /// </summary>
    /// <param name="newState"></param>
    public void LockApplication(bool newState = true)
    {
        _applicationLockState = newState;
    }
}