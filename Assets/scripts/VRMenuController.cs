using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRMenuController : MonoBehaviour
{
    public InputActionAsset inputActions;
    public string actionMapName = "Gameplay";
    public string menuActionName = "Menu";
    public Transform headTransform;
    public Canvas menuCanvas;

    private InputAction menuAction;
    private bool isMenuOpen;

    void Start()
    {
        menuCanvas.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        var map = inputActions.FindActionMap(actionMapName);
        menuAction = map?.FindAction(menuActionName);
        if (menuAction != null)
        {
            menuAction.performed += ToggleMenu;
            menuAction.Enable();
        }
    }

    void OnDisable()
    {
        if (menuAction != null)
        {
            menuAction.performed -= ToggleMenu;
            menuAction.Disable();
        }
    }

    void ToggleMenu(InputAction.CallbackContext ctx)
    {
        isMenuOpen = !isMenuOpen;
        menuCanvas.gameObject.SetActive(isMenuOpen);

        if (isMenuOpen)
        {
            // Colocar el menú frente a la vista
            Vector3 pos = headTransform.position + headTransform.forward * 2f;
            menuCanvas.transform.position = pos;
            menuCanvas.transform.LookAt(headTransform);
            menuCanvas.transform.Rotate(0, 180, 0); // voltear
        }
    }
}
