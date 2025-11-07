using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientAudioManager : MonoBehaviour
{
    public static AmbientAudioManager instance;

    [Header("Audio Ambiental")]
    public AudioSource ambientSource;
    public float fadeSpeed = 1.5f;
    private float originalVolume;

    void Awake()
    {
        // Singleton para acceso global
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (ambientSource != null)
        {
            originalVolume = ambientSource.volume;
            ambientSource.loop = true;
            ambientSource.Play();
        }
    }

    public void BajarVolumen()
    {
        StopAllCoroutines();
        StartCoroutine(FadeTo(0f));
    }

    public void SubirVolumen()
    {
        StopAllCoroutines();
        StartCoroutine(FadeTo(originalVolume));
    }

    private System.Collections.IEnumerator FadeTo(float targetVolume)
    {
        while (Mathf.Abs(ambientSource.volume - targetVolume) > 0.01f)
        {
            ambientSource.volume = Mathf.MoveTowards(ambientSource.volume, targetVolume, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        ambientSource.volume = targetVolume;
    }
}

