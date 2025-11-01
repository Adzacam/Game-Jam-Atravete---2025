using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GazeButton : MonoBehaviour
{
    public float gazeTime = 2f;
    public UnityEvent onGazeComplete;
    public Image progressImage; // asigna una imagen circular o barra

    private float timer;
    private bool gazing;

    public void OnPointerEnter() => gazing = true;
    public void OnPointerExit()
    {
        gazing = false;
        timer = 0;
        if (progressImage) progressImage.fillAmount = 0;
    }

    void Update()
    {
        if (gazing)
        {
            timer += Time.deltaTime;
            if (progressImage)
                progressImage.fillAmount = timer / gazeTime;

            if (timer >= gazeTime)
            {
                onGazeComplete.Invoke();
                timer = 0;
                gazing = false;
                if (progressImage) progressImage.fillAmount = 0;
            }
        }
    }
}
