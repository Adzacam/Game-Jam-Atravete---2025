using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;

public interface IInteractable
{
    void Interact();
}

public class Interactor : MonoBehaviour
{
    public InputActionAsset inputActions;
    public string actionMapName = "Gameplay";
    public string interactActionName = "Interact";

    public Transform headTransform; // cámara
    public float maxDistance = 3f;
    public LayerMask interactLayer = ~0;

    InputAction interactAction;

    void OnEnable()
    {
        var map = inputActions.FindActionMap(actionMapName);
        interactAction = map?.FindAction(interactActionName);
        if (interactAction != null)
        {
            interactAction.performed += OnInteractPerformed;
            interactAction.Enable();
        }
    }

    void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.performed -= OnInteractPerformed;
            interactAction.Disable();
        }
    }

    void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        Ray ray = new Ray(headTransform.position, headTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactLayer))
        {
            var interactable = hit.collider.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                return;
            }

            // Si no implementa la interfaz, puedes llamar a un método común:
            var mono = hit.collider.GetComponentInParent<MonoBehaviour>();
            if (mono != null)
            {
                // ejemplo: buscar componente con método público OnInteract (opcional)
                mono.SendMessage("OnInteract", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    // Opcional: dibujar raycast en editor
    void OnDrawGizmosSelected()
    {
        if (headTransform == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(headTransform.position, headTransform.position + headTransform.forward * maxDistance);
    }
}
