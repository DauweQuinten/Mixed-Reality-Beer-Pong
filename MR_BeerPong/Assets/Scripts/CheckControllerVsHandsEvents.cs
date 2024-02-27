using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This component will check if the user is using hand tracking or a touch controller.
/// An event will be invoked when the user starts using hand tracking, touch controller or when no controller is found.
/// </summary>
public class CheckControllerVsHandsEvents : MonoBehaviour
{
    public UnityEvent OnHandTrackingStarted;
    public UnityEvent OnTouchControllerActivated;
    public UnityEvent OnNoActiveControllerFound;

    private bool _isHandTrackingActive = false;
    private bool _isTouchControllerActivated = false;


    // Update is called once per frame
    void Update()
    {
        if (OVRInput.IsControllerConnected(OVRInput.Controller.Hands))
        {
            if (!_isHandTrackingActive)
            {
                _isHandTrackingActive = true;
                _isTouchControllerActivated = false;
                OnHandTrackingStarted.Invoke();
            }
        }
        else if (OVRInput.IsControllerConnected(OVRInput.Controller.Touch))
        {
            if (!_isTouchControllerActivated)
            {
                _isTouchControllerActivated = true;
                _isHandTrackingActive = false;
                OnTouchControllerActivated.Invoke();
            }
        }
        else
        {
            if(_isTouchControllerActivated || _isHandTrackingActive)
            {
                _isTouchControllerActivated = false;
                _isHandTrackingActive = false;
                OnNoActiveControllerFound.Invoke();
            }
        }
    }
}
