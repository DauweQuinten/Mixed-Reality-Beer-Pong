using System;
using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This component makes an object grabbable.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class GrabbableObjectWithControllers : MonoBehaviour
{
    private bool _isGrabbed;
    private DirectInteractorWithControllers _interactor;

    private void SetGrabbedState(bool isGrabbed, DirectInteractorWithControllers interactor)
    {
        if (_isGrabbed && isGrabbed)
        {
            _interactor.ResetInteractor();
        }
        _isGrabbed = isGrabbed;
        _interactor = interactor;
    }

    /// <summary>
    /// Grab this object
    /// </summary>
    public void Grab(DirectInteractorWithControllers interactor)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        transform.SetParent(interactor.transform);
        SetGrabbedState(true, interactor);
    }

    /// <summary>
    /// Release this object with a given velocity
    /// </summary>
    /// <param name="interactor"></param>
    /// <param name="releaseVelocity"></param>
    public void Release(DirectInteractorWithControllers interactor, Vector3 releaseVelocity)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = releaseVelocity;
        rb.transform.SetParent(null);
        SetGrabbedState(false, interactor);
    }
}
