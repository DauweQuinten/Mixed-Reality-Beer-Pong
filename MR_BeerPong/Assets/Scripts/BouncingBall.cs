using Oculus.Interaction.Surfaces;
using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

/// <summary>
/// This component represents a ball that will bounce of a bouncy surface.
/// It also takes into account the velocity of the surface it bounces off of.
/// If the surface is moving towards the ball, the ball will bounce off with a higher velocity. 
/// </summary>
public class BouncingBall : MonoBehaviour
{
    [SerializeField, Tooltip("If true, the velocity vectors will be drawn in the inspector.")]
    private bool _drawDebugVectors = false;
    private Rigidbody _ballRb;
    private Vector3 _ballVelocity = Vector3.zero;
    private bool _canBounce = false;

    private void Start()
    {
        // Give the object time to spawn without bouncing away
        StartCoroutine(EnableBounceAfterSeconds(1f));
        _ballRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_ballRb)
        {
            _ballVelocity = _ballRb.velocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out bouncySurface surface) && _canBounce)
        {
            surface.PlayAudio();
            GameObject collisionObject = collision.gameObject;

            if (collisionObject != null)
            {
                Vector3 bounceVelocity = CalculateBounceVelocity(collision, _ballVelocity, surface);
                Vector3 surfaceVelocity = surface.GetSurfaceVelocity();
                Vector3 outgoingVelocity = bounceVelocity + surfaceVelocity;
                if (surface.compensateFastMovingCollisions)
                {
                    transform.position += outgoingVelocity.normalized * 0.1f;
                }

                _ballRb.velocity = outgoingVelocity;
            }
        }
    }

    Vector3 CalculateBounceVelocity(Collision collision, Vector3 velocityBeforeCollision, bouncySurface surface)
    {
        Vector3 normal = collision.GetContact(0).normal;
        Vector3 velocityAfterBounce = Vector3.Reflect(velocityBeforeCollision, normal);

        if (_drawDebugVectors)
        {
            DebugDrawDirectionOfVector(collision.GetContact(0).point, normal, Color.blue, 1f);
            DebugDrawDirectionOfVector(collision.GetContact(0).point, velocityBeforeCollision, Color.green, 1f);
            DebugDrawDirectionOfVector(collision.GetContact(0).point, velocityAfterBounce, Color.red, 1f);
        }

        return velocityAfterBounce * surface.GetBounciness();
    }

    private void DebugDrawDirectionOfVector(Vector3 startPosition, Vector3 direction, Color color, float scale = float.PositiveInfinity)
    {
        Debug.DrawLine(startPosition, (startPosition + direction) * scale, color, float.PositiveInfinity);
    }

    IEnumerator EnableBounceAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        _canBounce = true;
    }
}
