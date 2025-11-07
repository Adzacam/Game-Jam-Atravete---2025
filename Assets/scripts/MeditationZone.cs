using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

namespace JardinSen
{
    public class MeditationZone : MonoBehaviour
    {
        [Header("Referencias")]
        public GameObject player;
        public PlayerMovement playerMovement;
        public Image fadeImage;                // Imagen negra pantalla completa
        public Text interactText;              // Texto "Presione R1 para meditar"
        public AudioSource ambienteTerror1;    // Voces terroríficas
        public AudioSource ambienteTerror2;    // Ambiente de fondo
        public AudioSource audioRelajante;     // Música relajante

        [Header("Duraciones")]
        public float fadeDuration = 2f;
        public float meditationTime = 6f;

        private bool playerInZone = false;
        private bool isMeditating = false;
        private InputAction interactAction;

        void Start()
        {
            // Acción de Interact
            var inputActions = playerMovement.inputActions;
            var map = inputActions.FindActionMap(playerMovement.actionMapName);
            interactAction = map.FindAction("Interact");

            // Asegurar texto desactivado al inicio
            if (interactText != null)
                interactText.gameObject.SetActive(false);

            // Asegurar que el fadeImage exista y esté activo
            if (fadeImage != null)
            {
                if (!fadeImage.gameObject.activeSelf)
                    fadeImage.gameObject.SetActive(true);

                // Asegurar transparencia inicial
                Color c = fadeImage.color;
                fadeImage.color = new Color(c.r, c.g, c.b, 0f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player && !isMeditating)
            {
                Debug.Log("Jugador entró al área de meditación");
                playerInZone = true;

                if (interactText != null)
                {
                    interactText.text = "Presione R1 para meditar";
                    interactText.gameObject.SetActive(true);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == player)
            {
                Debug.Log("Jugador salió del área de meditación");
                playerInZone = false;

                if (interactText != null)
                    interactText.gameObject.SetActive(false);
            }
        }

        void Update()
        {
            if (playerInZone && !isMeditating && interactAction != null && interactAction.triggered)
            {
                StartCoroutine(StartMeditation());
            }
        }

        private IEnumerator StartMeditation()
        {
            isMeditating = true;
            Debug.Log("Meditación iniciada");

            if (interactText != null)
                interactText.gameObject.SetActive(false);

            // Desactivar movimiento del jugador
            playerMovement.enabled = false;

            // Cambiar sonidos
            if (ambienteTerror1 != null && ambienteTerror1.isPlaying) ambienteTerror1.Stop();
            if (ambienteTerror2 != null && ambienteTerror2.isPlaying) ambienteTerror2.Stop();
            if (audioRelajante != null && !audioRelajante.isPlaying) audioRelajante.Play();

            // Fundido a negro
            yield return StartCoroutine(FadeScreen(1f, fadeDuration));

            // Mantener meditación
            yield return new WaitForSeconds(meditationTime);

            // Fundido de vuelta
            yield return StartCoroutine(FadeScreen(0f, fadeDuration));

            // Reactivar movimiento
            playerMovement.enabled = true;

            // Restaurar sonidos originales
            if (audioRelajante != null && audioRelajante.isPlaying) audioRelajante.Stop();
            if (ambienteTerror1 != null && !ambienteTerror1.isPlaying) ambienteTerror1.Play();
            if (ambienteTerror2 != null && !ambienteTerror2.isPlaying) ambienteTerror2.Play();

            isMeditating = false;
            Debug.Log("Meditación terminada");
        }

        private IEnumerator FadeScreen(float targetAlpha, float duration)
        {
            if (fadeImage == null) yield break;

            float startAlpha = fadeImage.color.a;
            float t = 0f;

            while (t < duration)
            {
                t += Time.deltaTime;
                float normalized = t / duration;
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, normalized);
                Color c = fadeImage.color;
                fadeImage.color = new Color(c.r, c.g, c.b, newAlpha);
                yield return null;
            }
        }
    }
}
