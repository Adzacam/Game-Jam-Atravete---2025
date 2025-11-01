using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OjosController : MonoBehaviour
{
    [Header("Material de los ojos")]
    public Material ojosMaterial;

    [Header("Duraciones (segundos)")]
    public float tiempoAbrir = 2f;
    public float tiempoCerrar = 1.5f;

    private float targetAlpha = 1f;
    private float velocidad = 1f;

    void Start()
    {
        if (ojosMaterial == null)
        {
            Debug.LogError("⚠️ No se asignó el material de los ojos.");
            return;
        }

        // Inicia con ojos cerrados
        Color c = ojosMaterial.color;
        c.a = 1f;
        ojosMaterial.color = c;
    }

    void Update()
    {
        // Transición gradual entre el alpha actual y el objetivo
        Color c = ojosMaterial.color;
        c.a = Mathf.MoveTowards(c.a, targetAlpha, velocidad * Time.deltaTime);
        ojosMaterial.color = c;
    }

    public void AbrirOjos()
    {
        targetAlpha = 0f;
        velocidad = 1f / tiempoAbrir;
    }

    public void CerrarOjos()
    {
        targetAlpha = 1f;
        velocidad = 1f / tiempoCerrar;
    }

    public bool EstanCerrados()
    {
        return ojosMaterial.color.a >= 0.95f;
    }

    public bool EstanAbiertos()
    {
        return ojosMaterial.color.a <= 0.05f;
    }
}

