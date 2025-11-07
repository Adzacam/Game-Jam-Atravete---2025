using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;

namespace JardinSen
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References")]
        public InputActionAsset inputActions; // Asignar PlayerControls en inspector
        public string actionMapName = "Gameplay";
        public string moveActionName = "Move";
        public Transform headTransform; // asignar la Cámara (Cardboard Main Camera)

        [Header("Movement")]
        public float moveSpeed = 2.5f;
        public float gravity = -9.81f;

        CharacterController cc;
        InputAction moveAction;
        Vector3 velocity;

        void Awake()
        {
            cc = GetComponent<CharacterController>();
            if (inputActions == null) Debug.LogError("Asigna el InputActionAsset (PlayerControls).");
        }

        void OnEnable()
        {
            var map = inputActions.FindActionMap(actionMapName);
            moveAction = map?.FindAction(moveActionName);
            moveAction?.Enable();
        }

        void OnDisable()
        {
            moveAction?.Disable();
        }

        void Update()
        {
            Vector2 input = moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;
            // input.x -> izquierdo/derecho, input.y -> adelante/atrás

            // Dirección relativa a la cabeza (solo en el plano XZ)
            Vector3 forward = headTransform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = headTransform.right;
            right.y = 0;
            right.Normalize();

            Vector3 move = forward * input.y + right * input.x;
            if (move.sqrMagnitude > 1f) move.Normalize();

            cc.Move(move * moveSpeed * Time.deltaTime);

            // gravity simple (opcional; si tu rig ya controla Y no lo uses)
            if (!cc.isGrounded)
            {
                velocity.y += gravity * Time.deltaTime;
                cc.Move(velocity * Time.deltaTime);
            }
            else
            {
                velocity.y = 0;
            }
        }
    }
}
