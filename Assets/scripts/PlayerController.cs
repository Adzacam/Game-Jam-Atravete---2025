using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Camera head;
    bool walking = false;
    [SerializeField] float MoveSpeed = 1.0f;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
        head = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        VrWalk();
    }

    void VrWalk()
    {
        if (walking == false && head.transform.eulerAngles.x>=30 && head.transform.eulerAngles.x<=45)
        {
            walking = true;
        }
        else if (walking == true && head.transform.eulerAngles.x<=30 && head.transform.eulerAngles.x<= 45
        || head.transform.eulerAngles.x>=45)
        {
            walking = false;
        }
        if (walking)
        {
            Vector3 forward = head.transform.forward;
            forward.y = 0;
            forward.Normalize();
            transform.position += forward * MoveSpeed * Time.deltaTime;
        }
    }
    
}
