using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    [SerializeField, Tooltip("Prefab to spawn.")]
    GameObject prefabToSpawn;
    [SerializeField, Tooltip("Transform to spawn at.")]
    Transform spawnLocation;
    [SerializeField, Tooltip("The box collider of the tabel, needed to calculate the new position based on the new scale of the tabel.")]
    BoxCollider tabelCollider;
    [SerializeField, Tooltip("Another box collider around the tabel, this will only indicate the size of the table prefab and will be disabled afterwards!")]
    BoxCollider tableBounds;
    [SerializeField, Tooltip("If true, debug spheres will spawn at the scaled spawn locations")]
    bool debugModeActive = false;
    [SerializeField, Tooltip("If true, the spawned object will be rescaled to fit the scaled tabel.")]
    bool rescaleSpawnedObjectSize = false;

    Vector3 initTableSize;
    Vector3 currentTableSize;
    Vector3 tableScaleValues;

    Vector3 CalculateScaleValues()
    {
        tableBounds.enabled = false;
        initTableSize = tableBounds.size;
        currentTableSize = tabelCollider.size;
        Vector3 scaleValues = new Vector3(
            initTableSize.x / currentTableSize.x,
            initTableSize.y / currentTableSize.y,
            initTableSize.z / currentTableSize.z);

        return scaleValues;
    }

    Vector3 CalculateScaledSpawnCoordinates()
    {
        Vector3 newSpawnPosition = Vector3.zero;
        Vector3 originalSpawnPosition = spawnLocation.localPosition;
        newSpawnPosition.x = originalSpawnPosition.x / tableScaleValues.x;
        newSpawnPosition.y = (originalSpawnPosition.y / tableScaleValues.y) - currentTableSize.y;
        newSpawnPosition.z = originalSpawnPosition.z / tableScaleValues.z;
        return newSpawnPosition;
    }

    public void RescaleSpawnCoordinates()
    {
        tableScaleValues = CalculateScaleValues();
        spawnLocation.position = CalculateScaledSpawnCoordinates();
        if(debugModeActive) DrawDebugSphere(spawnLocation.position);  
    }

    void DrawDebugSphere(Vector3 position)
    {
        GameObject debugSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        if(debugSphere.TryGetComponent(out SphereCollider collider)) collider.enabled = false;
        debugSphere.transform.SetParent(transform);
        debugSphere.transform.localScale = Vector3.one * 0.1f;
        debugSphere.transform.localPosition = position;
    }

    void Rescale(GameObject spawned)
    {
        if (spawned != null)
        {
            Vector3 currentSize = spawned.transform.localScale;
            Vector3 newSize = new Vector3(currentSize.x / tableScaleValues.x, currentSize.y / tableScaleValues.y, currentSize.z / tableScaleValues.z);
            spawned.transform.localScale = newSize;

            Debug.Log($"Prefab size: {currentSize}");
            Debug.Log($"New size: {newSize}");
        }
    }

    public GameObject SpawnObjectAtRescaledSpawnPoint()
    {
        RescaleSpawnCoordinates();
        
        if (prefabToSpawn == null)
        {
            return null;
        }

        GameObject spawnedObject = Instantiate(prefabToSpawn);
        spawnedObject.transform.SetParent(transform);
        spawnedObject.transform.localPosition = spawnLocation.position;
        spawnedObject.transform.rotation = spawnLocation.rotation;

        if (rescaleSpawnedObjectSize) Rescale(spawnedObject);
        return spawnedObject;
    }
}
