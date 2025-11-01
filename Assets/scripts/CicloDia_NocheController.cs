using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CicloDia_NocheController : MonoBehaviour
{
    [Range(0.0f,24f)]public float Hora = 12;
    public Transform Sol;
    //Duracion del dia en minutos
    public float DuracionDiaMin = 10;
    private float SolX;

    void Start()
    {
        
    }

    void Update()
    {
        RotacionSol();
        //Avance del tiempo
        Hora += Time.deltaTime * (24 / (60 * DuracionDiaMin));
        if(Hora >= 24)
        {
            Hora = 0;
        }
    }

//Rotacion del sol
    void RotacionSol()
   {
        SolX = 15f * Hora;

        Sol.localEulerAngles = new Vector3(SolX, 0, 0);
        //Control de intensidad de luz
        if(Hora > 6 || Hora < 18)
        {
            Sol.GetComponent<Light>().intensity = 0;
        }
        else
        {
            Sol.GetComponent<Light>().intensity = 1;
        }
   }
}
