using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

/// <summary>
/// This component allows you to select MRUK anchors with a touch controller.
/// </summary>
public class ControllerRaySelector : MonoBehaviour
{
    #region inspector variables

    [SerializeField, Tooltip("Only select MRUK anchors with these labels")]
    private MRUKAnchor.SceneLabels _validLabels;
    [SerializeField, Tooltip("This material gets applied to the MRUK anchor on hover. Let empty if you don't want to highlight the anchor")]
    private Material _highlightMaterial = null;
    [SerializeField, Tooltip("Toggle between left and right controller")]
    private bool _isLeftHanded = false;
    [SerializeField, Tooltip("if true, the last selected anchor stays highlighted until another anchor gets selected")]
    private bool _keepLastValidSelected = false;
    
    [SerializeField]
    private UnityEvent<MRUKAnchor> OnAnchorClicked;
    private GameObject _sphereIndicator = null;

    #endregion

    #region private variables
    private OVRInput.Controller _controller = OVRInput.Controller.RTouch;
    private MRUKAnchor _currentSelectedAnchor = null;
    private MRUKAnchor _prevSelectedAnchor = null;
    private MRUKAnchor _lastSelectedAnchor = null;
    private MRUKAnchor _prevLastSelectedAnchor = null;
    #endregion

    private void Start()
    {
        SetController(_isLeftHanded);
        ClearSelection();
    }

    void Update()
    {
        CheckMRUKRayCastFromController(_controller, out RaycastHit hit, out MRUKAnchor anchorHit);
        DrawIndicatorSphere(hit.point);
        HandleAnchorSelection(anchorHit);
        HandleAnchorHighlighting();
        HandleInputEvents();
    }

    private void OnDisable()
    {
        HighlightMRUKAnchor(GetSelectedAnchor(), false);
        Destroy(_sphereIndicator);
    }

    #region Handler functions

    void HandleAnchorSelection(MRUKAnchor anchorHit)
    {
        if (anchorHit != null)
        {
            bool anchorIsValid = _validLabels.HasFlag(anchorHit.GetLabelsAsEnum());
            if (anchorIsValid)
            {
                _currentSelectedAnchor = anchorHit;
                _lastSelectedAnchor = anchorHit;
            }
            else
            {
                _currentSelectedAnchor = null;
            }
        }
        else
        {
            _currentSelectedAnchor = null;
        }
    }

    void HandleAnchorHighlighting()
    {
        if (_highlightMaterial != null)
        {
            if (_keepLastValidSelected)
            {
                if (_lastSelectedAnchor != _prevLastSelectedAnchor)
                {
                    HighlightMRUKAnchor(_lastSelectedAnchor, true);
                    HighlightMRUKAnchor(_prevLastSelectedAnchor, false);
                    _prevLastSelectedAnchor = _lastSelectedAnchor;
                }
            }
            else
            {
                if (_currentSelectedAnchor != _prevSelectedAnchor)
                {
                    HighlightMRUKAnchor(_prevSelectedAnchor, false);
                    HighlightMRUKAnchor(_currentSelectedAnchor, true);
                    _prevSelectedAnchor = _currentSelectedAnchor;
                }
            }
        }
    }

    void HandleInputEvents()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            MRUKAnchor anchor = GetSelectedAnchor();
            if(anchor != null)
            {
                OnAnchorClicked.Invoke(anchor);
            }
        }
    }


    #endregion

    #region helper functions
    void CheckMRUKRayCastFromController(OVRInput.Controller controller, out RaycastHit hit, out MRUKAnchor anchorHit)
    {
        // Create a ray from the controller to the forward direction
        Vector3 rayOrigin = OVRInput.GetLocalControllerPosition(controller);
        Vector3 rayDirection = OVRInput.GetLocalControllerRotation(controller) * Vector3.forward;
        Ray ray = new Ray(rayOrigin, rayDirection);
        hit = new RaycastHit();
        anchorHit = null;
        MRUK.Instance?.GetCurrentRoom()?.Raycast(ray, Mathf.Infinity, out hit, out anchorHit);
    }

    void SetController(bool isLeftHanded)
    {
        if (isLeftHanded)
        {
            _controller = OVRInput.Controller.LTouch;
        }
        else
        {
            _controller = OVRInput.Controller.RTouch;
        }
    }

    void HighlightMRUKAnchor(MRUKAnchor anchor, bool isHighlighted)
    {
        if (!_highlightMaterial) return;
        
        MeshRenderer[] renderers = anchor?.gameObject.GetComponentsInChildren<MeshRenderer>();
        if (!anchor || renderers.Length < 1) return;

        if (isHighlighted)
        {
            foreach(MeshRenderer renderer in renderers)
            {
                renderer.material = _highlightMaterial;
                renderer.enabled = true;
            }
        }
        else
        {
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.material = _highlightMaterial;
                renderer.enabled = false;
            }
        }
    }

    void ClearSelection()
    {
        if(_currentSelectedAnchor != null)
        {
            _currentSelectedAnchor = null;
            HighlightMRUKAnchor(_currentSelectedAnchor, false);
        }
    }

    void DrawIndicatorSphere(Vector3 position)
    {
        if (!_sphereIndicator)
        {
            _sphereIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _sphereIndicator.transform.localScale *= 0.1f;
            _sphereIndicator.GetComponent<MeshRenderer>().material.color = Color.cyan;
        }
        _sphereIndicator.transform.position = position;
    }

    #endregion

    #region public functions

    /// <summary>
    /// Get the currently hovered valid MRUK anchor
    /// </summary>
    /// <returns></returns>
    public MRUKAnchor GetCurrentHoveredAnchor()
    {
        return _currentSelectedAnchor;
    }

    /// <summary>
    /// Get the last hovered valid MRUK anchor
    /// </summary>
    /// <returns></returns>
    public MRUKAnchor GetLastHoveredAnchor()
    {
        return _lastSelectedAnchor;
    }

    /// <summary>
    /// Get the currently selected MRUK anchor
    /// </summary>
    /// <returns></returns>
    public MRUKAnchor GetSelectedAnchor()
    {
        return _keepLastValidSelected ? _lastSelectedAnchor : _currentSelectedAnchor;     
    }

    #endregion
}
