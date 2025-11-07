using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [Header("Input")]
    public InputActionAsset inputActions;
    public string mapName = "Gameplay";
    public string pauseAction = "Pause";
    public string diaryAction = "OpenDiary";

    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject diaryMenu;
    

    private bool isPaused;
    private bool isDiaryOpen;

    InputAction pauseInput;
    InputAction diaryInput;

    void OnEnable()
{
    if (inputActions == null)
    {
        Debug.LogError("⚠️ MenuManager: No se asignó el InputActionAsset en el Inspector.");
        return;
    }

    var map = inputActions.FindActionMap(mapName);
    if (map == null)
    {
        Debug.LogError($"⚠️ No se encontró el Action Map '{mapName}' en {inputActions.name}");
        return;
    }

    pauseInput = map.FindAction(pauseAction);
    diaryInput = map.FindAction(diaryAction);

    if (pauseInput == null || diaryInput == null)
    {
        Debug.LogError($"⚠️ Acciones '{pauseAction}' o '{diaryAction}' no encontradas en {map.name}");
        return;
    }

    pauseInput.Enable();
    diaryInput.Enable();

    pauseInput.performed += ctx => TogglePause();
    diaryInput.performed += ctx => ToggleDiary();
}


    void OnDisable()
    {
        pauseInput.performed -= ctx => TogglePause();
        diaryInput.performed -= ctx => ToggleDiary();
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        diaryMenu.SetActive(false);
       

        Time.timeScale = isPaused ? 0 : 1;
    }

    public void ToggleDiary()
    {
        if (isPaused) return; 
        isDiaryOpen = !isDiaryOpen;
        diaryMenu.SetActive(isDiaryOpen);
        
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
