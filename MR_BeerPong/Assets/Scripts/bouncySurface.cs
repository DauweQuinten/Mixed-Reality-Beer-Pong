using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// This class represents a surface that can be bouncy.
/// This component will make other objects bounce off of it if that object contains a script that allows this!.
/// The bounciness is comparable with the bounciness of a physics material but does also detect collisions
/// with a grabbed object in a Mixed Reality environment.
/// It's important to know that this component has to be used in combination with another class like BouncingBall.
/// </summary>
public class bouncySurface : MonoBehaviour
{
    [SerializeField, Range(0, 1), Tooltip("Value of bounciness.")]
    private float _bounciness = 0.5f;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _prevPos = Vector3.zero;
    private AudioSource _audioSource = null;
    [SerializeField, Tooltip("Recalculate the position of the collided object.")]
    private bool _compensateFastMovingCollisions;

    public bool compensateFastMovingCollisions
    {
        get
        {
            return _compensateFastMovingCollisions;
        }
    }

    private void Start()
    {
        TryGetComponent(out _audioSource);
    }

    private void FixedUpdate()
    {
        _velocity = (transform.position - _prevPos) / Time.deltaTime;
        _prevPos = transform.position;
    }


    //private void OnCollisionEnter(Collision collision)
    //{
    //    _isBouncing = true;
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    _isBouncing = false;
    //}
    

    /// <summary>
    /// Get the current bounciness of the surface
    /// </summary>
    /// <returns></returns>
    public float GetBounciness()
    {
        return _bounciness;
    }

    /// <summary>
    /// Set the bounciness of the surface and return the new value.
    /// The returned value will be -1 if the value is out of range.
    /// </summary>
    /// <param name="value">new bounciness value</param>
    /// <returns></returns>
    public float SetBounciness(float value)
    {
        if (value > 1 || value < 0) return -1f;

        _bounciness = value;
        return _bounciness;
    }

    /// <summary>
    /// Get the current velocity of the surface
    /// </summary>
    /// <returns></returns>
    public Vector3 GetSurfaceVelocity()
    {
        return _velocity;
    }
    
    /// <summary>
    /// Get the current speed of the surface
    /// </summary>
    /// <returns></returns>
    public float GetSurfaceSpeed()
    {
        return _velocity.magnitude;
    }

    /// <summary>
    /// Play audio if this surface has an audiosource
    /// </summary>
    public void PlayAudio()
    {
        if (_audioSource == null) return;
        _audioSource.Play();
    }
}
