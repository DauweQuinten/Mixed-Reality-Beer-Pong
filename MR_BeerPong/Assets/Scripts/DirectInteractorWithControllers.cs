using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component allows you to grab objects
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class DirectInteractorWithControllers : MonoBehaviour
{
    private List<GameObject> _objectsInRange = new List<GameObject>();
    private GrabbableObjectWithControllers _grabbable = null;
    private Vector3 _controllerVelocity = Vector3.zero;
    private Vector3 _controllerPrevPos = Vector3.zero;

    [SerializeField, Tooltip("Multiplier of force of throw when released")]
    private float _velocityMultiplier = 1f;

    /// <summary>
    /// Grab the object in range
    /// </summary>
    public void Grab()
    {
        if (_grabbable || _objectsInRange.Count == 0) return;
        
        GameObject objectToGrab = _objectsInRange.ToArray()[0];
        if(objectToGrab.TryGetComponent(out _grabbable))
        {
            _grabbable.Grab(this);       
        }  
    }

    /// <summary>
    /// Release the grabbed object
    /// </summary>
    public void Release()
    {
        if (_grabbable == null) return;
        _grabbable.Release(this, _controllerVelocity * _velocityMultiplier);
        _grabbable = null;
    }

    /// <summary>
    /// Reset the state of the interactor. This is needed if the object is destroyed or grabbed by another DirectInteractor.
    /// </summary>
    public void ResetInteractor()
    {
        _grabbable = null;
    }

    private void Update()
    {
        _controllerVelocity = (transform.position - _controllerPrevPos) / Time.deltaTime;
        _controllerPrevPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out GrabbableObjectWithControllers grabbableInRange))
        {
            if (!_objectsInRange.Contains(other.gameObject))
            {
                _objectsInRange.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_objectsInRange.Contains(other.gameObject))
        {
            _objectsInRange.Remove(other.gameObject);
        }
    }
}
