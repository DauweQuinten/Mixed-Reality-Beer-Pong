using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeerPongCup : MonoBehaviour
{
    public UnityEvent onBallEntered = new UnityEvent();
    private AudioSource _audioSource;
    public AudioClip drinkAudio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            onBallEntered.Invoke();
            DrinkBeerSequence();
            StartCoroutine(DestroyCupAfterSeconds(6f));
        }
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
