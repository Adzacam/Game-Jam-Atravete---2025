using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    public OjosController ojos;
    public AudioSource ambiente;
    public AudioClip vientoHojas;
    public AudioClip vozLeo1;
    public AudioClip vozLeo2;
    public GameObject sen; // zorro azul
    public GameObject mision1Trigger; // para activarla al final

    void Start()
    {
        StartCoroutine(SecuenciaIntro());
    }

    IEnumerator SecuenciaIntro()
    {
        // 1. Ambiente base
        AmbientAudioManager.instance?.BajarVolumen();

        // 🔸 1. Pantalla negra (ojos cerrados)
        yield return new WaitForSeconds(1f);

        // 🔸 2. Sonido ambiental
        if (ambiente && vientoHojas)
        {
            ambiente.clip = vientoHojas;
            ambiente.Play();
        }

        // 🔸 3. Abrir ojos lentamente
        ojos.AbrirOjos();
        yield return new WaitUntil(() => ojos.EstanAbiertos());

        // 🔸 4. Voz de Leo
        if (vozLeo1)
        {
            ambiente.PlayOneShot(vozLeo1);
            yield return new WaitForSeconds(vozLeo1.length + 0.5f);
        }

        if (vozLeo2)
        {
            ambiente.PlayOneShot(vozLeo2);
            yield return new WaitForSeconds(vozLeo2.length + 0.5f);
        }
        AmbientAudioManager.instance?.SubirVolumen();
        // 🔸 5. Mostrar Sen
        if (sen != null) sen.SetActive(true);

        // 🔸 6. Activar misión 1
        if (mision1Trigger != null) mision1Trigger.SetActive(true);

        Debug.Log("✨ Intro terminada, misión 1 activa.");
    }
}
