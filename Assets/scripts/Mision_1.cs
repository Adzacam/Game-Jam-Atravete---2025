using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Mision_1 : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject farolPrincipal;
    public List<Light> otrosFaroles; // Asigna la luz o punto de luz de cada farol
    public Canvas canvasInteract;
    public Canvas canvasMision1;
    public AudioSource audioRecuerdo;
    public AudioClip clipPadre;
    public AudioClip clipLeo;

    [Header("Diario y Progreso")]
    public GameObject diaryMenu;
    public GameObject panelRecuerdo1;

    public OjosController ojos;

    private bool jugadorCerca = false;
    private bool misionCompletada = false;

    [Header("Input")]
    public InputActionAsset inputActions;
    private InputAction interactAction;

    void Start()
    {
        canvasInteract.gameObject.SetActive(false);
        canvasMision1.gameObject.SetActive(false);

        // Configurar input
        var map = inputActions.FindActionMap("Gameplay");
        if (map != null)
        {
            interactAction = map.FindAction("Interact");
            if (interactAction != null)
                interactAction.performed += ctx => IntentarReparar();
        }

        // Apagar faroles al inicio
        foreach (var f in otrosFaroles)
        {
            if (f != null) f.enabled = false;
        }
    }

    void OnEnable()
    {
        interactAction?.Enable();
    }

    void OnDisable()
    {
        interactAction?.Disable();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !misionCompletada)
        {
            jugadorCerca = true;
            canvasInteract.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            canvasInteract.gameObject.SetActive(false);
        }
    }

    void IntentarReparar()
    {
        if (!jugadorCerca || misionCompletada) return;

        StartCoroutine(SecuenciaMision());
    }

    IEnumerator SecuenciaMision()
    {
        // Cerrar ojos antes del recuerdo
        if (ojos != null) ojos.CerrarOjos();
            yield return new WaitUntil(() => ojos.EstanCerrados());
            yield return new WaitForSeconds(0.5f);

        misionCompletada = true;
        canvasInteract.gameObject.SetActive(false);
        canvasMision1.gameObject.SetActive(true);
        AmbientAudioManager.instance.BajarVolumen();
        // Reproducir recuerdo
        if (audioRecuerdo && clipPadre)
        {
            audioRecuerdo.clip = clipPadre;
            audioRecuerdo.Play();
            yield return new WaitForSeconds(audioRecuerdo.clip.length + 1f);
        }

        if (audioRecuerdo && clipLeo)
        {
            audioRecuerdo.clip = clipLeo;
            audioRecuerdo.Play();
            yield return new WaitForSeconds(audioRecuerdo.clip.length + 1f);
        }

         // Abrir ojos después del recuerdo
        if (ojos != null) ojos.AbrirOjos();
        yield return new WaitUntil(() => ojos.EstanAbiertos()); 

        // Encender el farol principal
        Light luz = farolPrincipal.GetComponentInChildren<Light>();
        if (luz != null) luz.enabled = true;

        // Esperar y encender los demás
        yield return new WaitForSeconds(3f);
        foreach (var f in otrosFaroles)
        {
            if (f != null) f.enabled = true;
        }
        AmbientAudioManager.instance.SubirVolumen();
        // Guardar en diario y progreso
        PlayerPrefs.SetInt("Mision1Completada", 1);
        PlayerPrefs.Save();

        if (panelRecuerdo1 != null)
            panelRecuerdo1.SetActive(true);

        yield return new WaitForSeconds(2f);
        canvasMision1.gameObject.SetActive(false);
    }
   void Update()
{
    // Control de luz segun hora
    CicloDia_NocheController ciclo = FindAnyObjectByType<CicloDia_NocheController>();
    if (ciclo != null)
    {
        bool esDeNoche = (ciclo.Hora >= 18f || ciclo.Hora < 6f);
        foreach (var f in otrosFaroles)
        {
            if (f != null) f.enabled = esDeNoche && misionCompletada;
        }

        if (farolPrincipal != null)
        {
            Light luz = farolPrincipal.GetComponentInChildren<Light>();
            if (luz != null) luz.enabled = esDeNoche && misionCompletada;
        }
    }
}

}

