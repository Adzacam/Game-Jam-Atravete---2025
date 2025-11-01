using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntoDescanso : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            PlayerPrefs.SetInt("DiaActual", PlayerPrefs.GetInt("DiaActual", 1) + 1);
            PlayerPrefs.Save();
            Debug.Log("🌙 Día guardado y siguiente iniciado");
        }
    }
}
