using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BeerPongCup : MonoBehaviour
{
    public UnityEvent onBallEntered = new UnityEvent();
    public UnityEvent onDrunk = new UnityEvent();
    private AudioSource _audioSource;
    public AudioClip drinkAudio;
    private bool _isHit = false;
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") && !_isHit)
        {
            _isHit = true;
            MakeGrabbable();
            onBallEntered.Invoke();
        }

        if (other.gameObject.CompareTag("Mouth") && _isHit)
        {
            DrinkBeerSequence();         
        }
    }


    private void MakeGrabbable()
    {
        Rigidbody rb;
        GrabbableObjectWithControllers grabbable;
        if (!TryGetComponent(out rb))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        if (!TryGetComponent(out grabbable))
        {
            grabbable = gameObject.AddComponent<GrabbableObjectWithControllers>();
        }

        rb.useGravity = false;
    }
    
    private void DrinkBeerSequence()
    {
        if(TryGetComponent(out _audioSource))
        {
            StartCoroutine(PlayAudioClip(drinkAudio));
        }
        else
        {
            Debug.LogWarning("There is no audio source assigned to the cup.");
        }
        onDrunk.Invoke();
        StartCoroutine(DestroyCupAfterSeconds(drinkAudio.length));
    }

    private IEnumerator PlayAudioClip(AudioClip clip, float delayInSeconds=0f)
    {     
        yield return new WaitForSeconds(delayInSeconds);
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    private IEnumerator DestroyCupAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
