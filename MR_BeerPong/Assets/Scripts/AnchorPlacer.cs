using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Use this class to place an anchor in the scene that has a PlaceableObject component.
/// </summary>
public class AnchorPlacer : MonoBehaviour
{

    [SerializeField] UnityEvent<GameObject> OnAnchorPlaced;

    /// <summary>
    /// Place an anchor in the scene with the help of the PlaceableObject component.
    /// </summary>
    /// <param name="anchor"></param>
    public void PlaceMRUKAnchor(MRUKAnchor anchor)
    {
        PlaceableObject placeable = anchor.GetComponentInChildren<PlaceableObject>();
        if (placeable)
        {
            GameObject placedObject = placeable.PlaceObject();
            OnAnchorPlaced.Invoke(placedObject);
        }
    }
}
